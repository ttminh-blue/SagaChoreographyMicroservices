using Microsoft.EntityFrameworkCore.Storage;
using OrderService.Data;
using OrderService.Repositories.IRepositories;

namespace OrderService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        public UnitOfWork(AppDbContext dbContext) => _dbContext = dbContext;

        public async Task<IDbContextTransaction> BeginTransactionAsync()
            => await _dbContext.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync()
            => await _dbContext.Database.CommitTransactionAsync();

        public async Task RollbackTransactionAsync()
            => await _dbContext.Database.RollbackTransactionAsync();
    }

}
