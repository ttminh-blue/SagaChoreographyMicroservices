using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OrderService.Data;
using OrderService.Repositories.IRepositories;

namespace OrderService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public IOutboxRepository OutboxMessages { get; private set; }

        public IOrderRepository Orders { get; private set; }

        public IOrderItemRepository OrderItems { get; private set; }

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            Orders = new OrderRepository(_dbContext);
            OrderItems = new OrderItemRepository(_dbContext);
            OutboxMessages = new OutboxRepository(_dbContext);
        }
        public int Complete()
        {
            return _dbContext.SaveChanges();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}