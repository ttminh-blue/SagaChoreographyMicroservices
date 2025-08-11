using OrderService.Models.Dtos;

namespace OrderService.Services.Interfaces
{
    public interface IOrderItemService
    {
        public Task<CreateOrderRequest> CreateOrderItems(CreateOrderRequest request);
    }
}
