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
                HostName = "localhost",
                Port = 5672,
                Password = "guest",
                UserName = "guest"
            };

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

        public Task<IChannel> GetChannelAsync() => _channelTask;
    }

}
