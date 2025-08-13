using Newtonsoft.Json;
using PaymentService.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PaymentService.Events
{
    public class EventListener : BackgroundService
    {
        private readonly RabbitMqChannelProvider _provider;
        private readonly IServiceScopeFactory _scopeFactory;
        public EventListener(RabbitMqChannelProvider provider, IServiceScopeFactory scopeFactory)
        {
            _provider = provider;
            _scopeFactory = scopeFactory;
        }

        protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
        {
            var channel = await _provider.GetChannelAsync();
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var order = JsonConvert.DeserializeObject<OrderEvent>(message);
                if (order != null)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
                    await paymentService.CreatePayment(order);
                }
            };

            await channel.BasicConsumeAsync(queue: "ordersEventsQueue", autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }
    }
}
