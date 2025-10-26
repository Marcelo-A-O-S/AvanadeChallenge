using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using StockService.Infrastructure.Workers;
namespace StockService.Infrastructure.Consumers
{
    public class ProductConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        public ProductConsumer(IConnectionFactory _factory)
        {
            this.factory = _factory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var connection = await this.factory.CreateConnectionAsync();
                var consumer = new RabbitMQConsumer(connection);
                consumer.RegisterHandler("", async message => { });
                await consumer.Start();
                Console.WriteLine("📡 Consumer de Produtos do RabbitMQ iniciado e aguardando mensagens...");
            }catch(Exception ex)
            {
                Console.WriteLine($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
    }
}