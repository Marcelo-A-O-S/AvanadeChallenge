using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SaleService.Infrastructure.Workers;

namespace SaleService.Infrastructure.Consumers
{
    public class OrderConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        public OrderConsumer(IConnectionFactory _factory)
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
                Console.WriteLine("📡 Consumer de Pedidos do RabbitMQ iniciado e aguardando mensagens...");
            }catch(Exception ex)
            {
                Console.WriteLine($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
    }
}