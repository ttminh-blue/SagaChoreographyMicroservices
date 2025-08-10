using CommonModels;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using OrderService.Events.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace OrderService.Events
{
    public class Publisher : IPublisher
    {
        private readonly RabbitMqChannelProvider _provider;

        public Publisher(RabbitMqChannelProvider provider)
        {
            _provider = provider;
        }

        public async void Publish(OrderEvent orderEvent)
        {
            var channel = await _provider.GetChannelAsync();

            var message = JsonConvert.SerializeObject(orderEvent);
            var body = Encoding.UTF8.GetBytes(message);
            var props = new BasicProperties();

            await channel.BasicPublishAsync("", "ordersEventsQueue", true, props, body);
        }
    }
}
