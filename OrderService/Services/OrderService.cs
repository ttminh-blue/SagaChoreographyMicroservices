using CommonModels;
using Newtonsoft.Json;
using OrderService.Events.Interfaces;
using OrderService.Models;
using OrderService.Models.Dtos;
using OrderService.Repositories.IRepositories;
using OrderService.Services.Interfaces;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPublisher _publisher;
        private readonly IOrderItemService _orderItemService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOutboxRepository _outboxRepository;
        public OrderService(IOrderRepository orderRepository, IPublisher publisher, IOrderItemService orderItemService, IUnitOfWork unitOfWork, IOutboxRepository outboxRepository)
        {
            _orderRepository = orderRepository;
            _publisher = publisher;
            _orderItemService = orderItemService;
            _unitOfWork = unitOfWork;
            _outboxRepository = outboxRepository;
        }

        public async Task<CreateOrderRequest> CreateOrder(CreateOrderRequest request)
        {
            var orderId = Guid.NewGuid();

            double orderAmount = request.OrderItems.Sum(item => item.Units * item.UnitPrice);

            var order = new Orders
            {
                Id = orderId,
                CustomerId = request.CustomerId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending,
                OrderAmount = orderAmount
            };
            using var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _orderRepository.Create(order);

                request.OrderID = orderId;
                await _orderItemService.CreateOrderItems(request);

                //_publisher.Publish(new OrderEvent
                //{
                //    CustomerId = request.CustomerId,
                //    OrderAmount = (int)orderAmount,
                //    OrderId = orderId
                //});

                OrderEvent orderEvent = new()
                {
                    CustomerId = request.CustomerId,
                    OrderAmount = (int)orderAmount,
                    OrderId = orderId
                };

                OutboxMessage outboxMessage = new()
                {
                    Id = Guid.NewGuid(),
                    OccurredOn = DateTime.UtcNow,
                    Type = nameof(OrderEvent),
                    Payload = JsonConvert.SerializeObject(orderEvent)
                };

                await _outboxRepository.Create(outboxMessage);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }


            return request;
        }

        public async Task<List<CreateOrderRequest>> GetAllOrders()
        {
            List<Orders> orders = await _orderRepository.GetAll();
            List<OrderItem> orderItems = await _orderItemService.GetAllOrderItems();

            var result = orders.Select(order => new CreateOrderRequest
            {
                CustomerId = order.CustomerId,
                OrderID = order.Id,
                OrderItems = orderItems
                            .Where(oi => oi.OrderId == order.Id)
                            .Select(oi => new OrderDTO
                            {
                                ProductId = oi.ProductId,
                                UnitPrice = oi.UnitPrice,
                                Units = oi.Units
                            })
                            .ToList()
            }).ToList();

            return result;
        }

        public async Task<CreateOrderRequest> GetOrder(Guid id)
        {
            Orders order = await _orderRepository.GetOne(x => x.Id == id);
            List<OrderItem> orderItem = await _orderItemService.GetOrderItem(id);

            CreateOrderRequest response = new()
            {
                CustomerId = order.CustomerId,
                OrderID = order.Id,
                OrderItems = orderItem
                            .Where(oi => oi.OrderId == order.Id)
                            .Select(oi => new OrderDTO
                            {
                                ProductId = oi.ProductId,
                                UnitPrice = oi.UnitPrice,
                                Units = oi.Units
                            })
                            .ToList()
            };

            return response;
        }

        public async Task<Guid> UpdateOrderStatus(Guid id, OrderStatus status)
        {
            Orders order = await _orderRepository.GetOne(x => x.Id == id);
            order.OrderStatus = status;

            await _orderRepository.Update(order);
            return id;
        }
    }
}