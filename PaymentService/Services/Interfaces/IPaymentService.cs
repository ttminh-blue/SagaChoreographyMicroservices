using PaymentService.Events;

namespace PaymentService.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<OrderEvent> CreatePayment(OrderEvent user);
    }
}
