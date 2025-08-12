using OrderService.Models;
using OrderService.Models.Dtos;

namespace OrderService.Services.Interfaces
{
    public interface IOrderItemService
    {
        public Task<CreateOrderRequest> CreateOrderItems(CreateOrderRequest request);
        public Task<List<OrderItem>> GetAllOrderItems();
        public Task<List<OrderItem>> GetOrderItem(Guid id);
    }
}
