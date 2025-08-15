using PaymentService.Data;
using PaymentService.Repositories.IRepositories;

namespace PaymentService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public IOutboxRepository OutboxMessages { get; private set; }
        public IPaymentRepository Payments { get; private set; }

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            Payments = new PaymentRepository(_dbContext);
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
