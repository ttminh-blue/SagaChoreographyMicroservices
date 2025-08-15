namespace PaymentService.Repositories.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IOutboxRepository OutboxMessages { get; }
        IPaymentRepository Payments { get; }
        int Complete();
    }
}
