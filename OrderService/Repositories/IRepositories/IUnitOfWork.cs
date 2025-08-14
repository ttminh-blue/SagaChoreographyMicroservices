using Microsoft.EntityFrameworkCore.Storage;

namespace OrderService.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }

}
