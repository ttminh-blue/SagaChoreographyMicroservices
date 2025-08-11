using CommonModels;
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
        public OrderService(IOrderRepository orderRepository, IPublisher publisher, IOrderItemService orderItemService)
        {
            _orderRepository = orderRepository;
            _publisher = publisher;
            _orderItemService = orderItemService;
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

            await _orderRepository.Create(order);

            request.OrderID = orderId;
            await _orderItemService.CreateOrderItems(request);

            _publisher.Publish(new OrderEvent
            {
                CustomerId = request.CustomerId,
                OrderAmount = (int)orderAmount,
                OrderId = orderId
            });

            return request;
        }
    }
}