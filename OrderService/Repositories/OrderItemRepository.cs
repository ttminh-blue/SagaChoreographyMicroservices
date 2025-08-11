using OrderService.Data;
using OrderService.Models;
using OrderService.Repositories.IRepositories;

namespace OrderService.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(AppDbContext db) : base(db)
        {
        }
    }
}
