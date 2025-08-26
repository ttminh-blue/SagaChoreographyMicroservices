
using CommonModels;
using Newtonsoft.Json;
using OrderService.Events.Interfaces;
using OrderService.Models;
using OrderService.Repositories.IRepositories;

namespace OrderService.Events
{
    public class PublishMessageListener : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IPublisher _publisher;
        public PublishMessageListener(IPublisher publisher, IServiceScopeFactory scopeFactory)
        {
            _publisher = publisher;
            _scopeFactory = scopeFactory;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
                var orderRepo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var orderItemRepo = scope.ServiceProvider.GetRequiredService<IOrderItemRepository>();
                var elasticSearchRepo = scope.ServiceProvider.GetRequiredService<IElasticsearchOrderRepository>();

                var messages = await outboxRepository.GetAll(
                    x => !x.Processed
                );

                foreach (var msg in messages)
                {
                    try
                    {
                        var eventObj = JsonConvert.DeserializeObject<OrderEvent>(msg.Payload);
                        if (eventObj != null)
                        {
                            _publisher.Publish(eventObj);
                            var order = await orderRepo.GetOne(x => x.Id == eventObj.OrderId);
                            if (order != null)
                            {
                                List<OrderItem> orderItems = await orderItemRepo.GetAll(x => x.OrderId == eventObj.OrderId);
                                order.OrderItems = orderItems;
                                await elasticSearchRepo.IndexOrderAsync(order);
                            }
                        }
                        msg.Processed = true;
                        msg.ProcessedOn = DateTime.UtcNow;
                        await outboxRepository.Update(msg);
                    }
                    catch
                    {
                    }
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
