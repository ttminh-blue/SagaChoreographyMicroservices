using RabbitMQ.Client.Events;
using System.Text;

namespace OrderService.Events
{
    public class EventListener : BackgroundService
    {
        private readonly RabbitMqChannelProvider _provider;
        public EventListener(RabbitMqChannelProvider provider)
        {
            _provider = provider;
        }

        protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
        {
            var channel = await _provider.GetChannelAsync();
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

            };

            //await channel.BasicConsumeAsync(queue: "customerEventsQueue", autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }
    }
}
