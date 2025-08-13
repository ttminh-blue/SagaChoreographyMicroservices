using PaymentService.Events;
using PaymentService.Events.Interfaces;
using PaymentService.Models;
using PaymentService.Repositories.IRepositories;
using PaymentService.Services.Interfaces;

namespace PaymentService.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPublisher _publisher;
        public PaymentService(IPaymentRepository paymentRepository, IPublisher publisher)
        {
            _paymentRepository = paymentRepository;
            _publisher = publisher;
        }

        public async Task<OrderEvent> CreatePayment(OrderEvent order)
        {
            string transactionId = Guid.NewGuid().ToString("N");

            Payment paymentRequest = new()
            {
                Id = Guid.NewGuid(),
                PaymentDate = DateTime.Now,
                OrderId = order.OrderId,
                Amount = order.OrderAmount,
                PaymentStatus = 1,
                PaymentMethod = "Cash",
                TransactionId = transactionId
            };

            await _paymentRepository.Create(paymentRequest);

            order.OrderStatus = OrderStatus.Paid;
            _publisher.Publish(order);

            return order;
        }
    }
}
