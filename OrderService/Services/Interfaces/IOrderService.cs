using OrderService.Models.Dtos;

namespace OrderService.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<CreateOrderRequest> CreateOrder(CreateOrderRequest user);
        public Task<List<CreateOrderRequest>> GetAllOrders();
        public Task<CreateOrderRequest> GetOrder(Guid id);

    }
}
