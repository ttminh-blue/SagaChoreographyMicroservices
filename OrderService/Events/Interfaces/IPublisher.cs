using CommonModels;

namespace OrderService.Events.Interfaces
{
    public interface IPublisher
    {
        void Publish(OrderEvent orderEvent);
    }
}
