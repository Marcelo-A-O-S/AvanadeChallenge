using System.Text.Json;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using StockService.Domain.Interfaces;
using StockService.Infrastructure.Workers;

namespace StockService.Infrastructure.Consumers
{
    public class StockMovementConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly IProductRepository productRepository;
        public StockMovementConsumer(
           IConnectionFactory _factory,
           IRabbitMQProducer _rabbitMQProducer,
           IProductRepository _productRepository)
        {
            this.factory = _factory;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.productRepository = _productRepository;
        }
        public async Task RegisterMovement(string message)
        {
            try
            {
                var data = JsonSerializer.Deserialize<>(message);
            }catch(Exception ex)
            {
                Console.WriteLine($"Error ")
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var connection = await this.factory.CreateConnectionAsync();
                var consumer = new RabbitMQConsumer(connection);
                consumer.RegisterHandler("add-order", async message => { await this.RegisterMovement(message);});
                await consumer.Start();
                Console.WriteLine("📡 Consumer da Movimentação de Estoque do RabbitMQ iniciado e aguardando mensagens...");
            }catch(Exception ex)
            {
                Console.WriteLine($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
    }
}