namespace PaymentService.Events.Interfaces
{
    public interface IPublisher
    {
        void Publish(OrderEvent paymentEvent);
    }
}
