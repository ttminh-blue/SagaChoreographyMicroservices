using PaymentService.Data;
using PaymentService.Models;
using PaymentService.Repositories.IRepositories;

namespace PaymentService.Repositories
{
    public class OutboxRepository : GenericRepository<OutboxMessage>, IOutboxRepository
    {
        public OutboxRepository(AppDbContext db) : base(db)
        {
        }
    }
}
