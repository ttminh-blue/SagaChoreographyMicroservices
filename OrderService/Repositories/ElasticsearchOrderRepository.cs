using Elastic.Clients.Elasticsearch;
using OrderService.Models;
using OrderService.Repositories.IRepositories;

namespace OrderService.Repositories
{
    public class ElasticsearchOrderRepository : IElasticsearchOrderRepository
    {
        private readonly ElasticsearchClient _client;

        public ElasticsearchOrderRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task IndexOrderAsync(Orders order)
        {
            var indexExists = await _client.Indices.ExistsAsync("orders");
            if (!indexExists.Exists)
            {
                await _client.Indices.CreateAsync("orders", c => c
                      .Mappings(m => m
                          .Properties<Orders>(p => p
                              .Keyword(k => k.Id)
                              .Date(k => k.OrderDate)
                              .IntegerNumber(q => q.CustomerId)
                              .DoubleNumber(k => k.OrderAmount)
                              .Keyword(t => t.OrderStatus)
                              .Nested("orderItems", nested => nested
                                  .Properties(itemProps => itemProps
                                      .Text("productName")
                                  )
                              )
                          )
                      ));
            }

            var response = await _client.IndexAsync(order, idx => idx
                        .Index("orders")
                        .Id(order.Id)
                    );

            if (!response.IsValidResponse)
            {
                // Handle error
                throw new Exception($"Failed to index order: {response.DebugInformation}");
            }
        }

        public async Task<Orders?> GetOrderByIdAsync(string id)
        {
            var response = await _client.GetAsync<Orders>(id, g => g.Index("orders"));
            return response.Found ? response.Source : null;
        }

        public async Task<IEnumerable<Orders>> SearchOrdersAsync(string keyword)
        {
            var response = await _client.SearchAsync<Orders>(s => s
                           .Indices("orders")
                           .Query(q => q
                               .Nested(n => n
                                   .Path("orderItems")
                                   .Query(nq => nq
                                       .Match(m => m
                                           .Field("orderItems.productName")
                                           .Query(keyword)
                                       )
                                   )
                               )
                           )
                       );


            return response.Documents;
        }
    }
}
