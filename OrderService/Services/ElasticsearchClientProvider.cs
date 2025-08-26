using Elastic.Clients.Elasticsearch;

namespace OrderService.Services
{
    public class ElasticsearchClientProvider
    {
        public ElasticsearchClient Client { get; }

        public ElasticsearchClientProvider(IConfiguration config)
        {
            var settings = new ElasticsearchClientSettings(new Uri(config["Elasticsearch:Uri"]))
                                .DefaultIndex(config["Elasticsearch:IndexName"]);

            Client = new ElasticsearchClient(settings);
        }
    }
}
