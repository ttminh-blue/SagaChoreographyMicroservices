using RabbitMQ.Client;

namespace OrderService.Events
{
    public class RabbitMqChannelProvider
    {
        private readonly Task<IChannel> _channelTask;

        public RabbitMqChannelProvider()
        {
            _channelTask = CreateChannelAsync();
        }

        private async Task<IChannel> CreateChannelAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                Port = 5672,
                Password = "guest",
                UserName = "guest"
            };


            const int maxRetries = 10;
            const int delayMs = 3000;

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    var connection = await factory.CreateConnectionAsync();
                    var channel = await connection.CreateChannelAsync();

                    await channel.QueueDeclareAsync(
                        queue: "ordersEventsQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    return channel;
                }
                catch (Exception ex)
                {
                    if (i == maxRetries - 1) throw;
                    await Task.Delay(delayMs);
                }
            }
            return null;
        }

        public Task<IChannel> GetChannelAsync() => _channelTask;
    }

}
