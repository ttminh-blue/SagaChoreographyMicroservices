using Newtonsoft.Json;
using PaymentService.Events;
using PaymentService.Events.Interfaces;
using PaymentService.Models;
using PaymentService.Repositories.IRepositories;
using PaymentService.Services.Interfaces;

namespace PaymentService.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPublisher _publisher;
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IUnitOfWork unitOfWork, IPublisher publisher)
        {
            _unitOfWork = unitOfWork;
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

            await _unitOfWork.Payments.Create(paymentRequest);

            order.OrderStatus = OrderStatus.Paid;

            OutboxMessage outboxMessage = new()
            {
                Id = Guid.NewGuid(),
                OccurredOn = DateTime.UtcNow,
                Type = nameof(OrderEvent),
                Payload = JsonConvert.SerializeObject(order)
            };

            await _unitOfWork.OutboxMessages.Create(outboxMessage);
            _unitOfWork.Complete();

            return order;
        }
    }
}
