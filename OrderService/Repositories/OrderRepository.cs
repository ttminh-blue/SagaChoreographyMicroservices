using OrderService.Data;
using OrderService.Models;
using OrderService.Repositories.IRepositories;

namespace OrderService.Repositories
{
    public class OrderRepository : GenericRepository<Orders>, IOrderRepository
    {
        public OrderRepository(AppDbContext db) : base(db)
        {
        }
    }
}
