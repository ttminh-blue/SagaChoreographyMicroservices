using OrderService.Models;

namespace OrderService.Repositories.IRepositories
{
    public interface IElasticsearchOrderRepository
    {
        public Task IndexOrderAsync(Orders order);

        public Task<Orders?> GetOrderByIdAsync(string id);

        public Task<IEnumerable<Orders>> SearchOrdersAsync(string keyword);
    }
}