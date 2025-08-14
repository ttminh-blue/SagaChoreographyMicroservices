using Microsoft.EntityFrameworkCore.Storage;

namespace OrderService.Repositories.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IOutboxRepository OutboxMessages { get; }
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }
        int Complete();
    }
}
