using OrderService.Models;

namespace OrderService.Repositories.IRepositories
{
    public interface IOutboxRepository : IGenericRepository<OutboxMessage>
    {
    }
}
