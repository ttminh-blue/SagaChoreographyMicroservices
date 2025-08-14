using OrderService.Data;
using OrderService.Models;
using OrderService.Repositories.IRepositories;

namespace OrderService.Repositories
{
    public class OutboxRepository : GenericRepository<OutboxMessage>, IOutboxRepository
    {
        public OutboxRepository(AppDbContext db) : base(db)
        {
        }
    }
}
