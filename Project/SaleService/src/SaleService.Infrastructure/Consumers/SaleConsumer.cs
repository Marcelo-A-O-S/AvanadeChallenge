using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SaleService.Infrastructure.Workers;

namespace SaleService.Infrastructure.Consumers
{
    public class SaleConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        public SaleConsumer(IConnectionFactory _factory)
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
                Console.WriteLine("📡 Consumer de Vendas do RabbitMQ iniciado e aguardando mensagens...");
            }catch(Exception ex)
            {
                Console.WriteLine($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
    }
}