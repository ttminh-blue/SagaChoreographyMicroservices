using OrderService.Models;
using OrderService.Models.Dtos;
using OrderService.Repositories.IRepositories;
using OrderService.Services.Interfaces;

namespace OrderService.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }
        public async Task<CreateOrderRequest> CreateOrderItems(CreateOrderRequest request)
        {
            var orderItems = request.OrderItems.Select(x => new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderID,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice,
                Units = x.Units
            });

            foreach (var item in orderItems)
            {
                await _orderItemRepository.Create(item);
            }

            return request;
        }
    }
}
