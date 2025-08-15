using PaymentService.Models;

namespace PaymentService.Repositories.IRepositories
{
    public interface IOutboxRepository : IGenericRepository<OutboxMessage>
    {
    }
}
