using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using StockService.Infrastructure.Workers;
using StockService.Domain.Entities;
using StockService.Infrastructure.Messaging.Contracts;
using StockService.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using StockService.Domain.Enums;
namespace StockService.Infrastructure.Messaging.Consumers
{
    public class ProductConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<ProductConsumer> logger;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public ProductConsumer(
            IConnectionFactory _factory,
            ILogger<ProductConsumer> _logger,
            IServiceScopeFactory _scopeFactory,
            IRabbitMQProducer _rabbitMQProducer)
        {
            this.factory = _factory;
            this.logger = _logger;
            this.scopeFactory = _scopeFactory;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        public async Task ValidateProduct(string message)
        {
            try
            {
                using var scope = this.scopeFactory.CreateScope();
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                var saleMessage = JsonSerializer.Deserialize<SaleMessage>(message);
                if (saleMessage != null)
                {
                    var product = await productRepository.GetById(saleMessage.ProductId);
                    if (product != null)
                    {
                        if (product.Quantity < saleMessage.Quantity)
                        {
                            await this.rabbitMQProducer.Publish("sale-not-stock", new ProductResponseMessage
                            {
                                SaleId = saleMessage.Id,
                                Status = SaleStatusResponse.NOT_STOCK.ToString(),
                                Message = "Não há estoque suficiente"
                            });
                            this.logger.LogInformation($"Não há estoque suficiente para o Id do produto: ${product.Id}");
                        }
                        else
                        {
                            await this.rabbitMQProducer.Publish("sale-valid", new ProductResponseMessage
                            {
                                SaleId = saleMessage.Id,
                                Status = SaleStatusResponse.PARTIALLY_CONFIRMED.ToString(),
                                Message = "Venda validada com sucesso!"
                            });
                            this.logger.LogInformation("Venda validada com sucesso!");
                        }
                    }
                    else
                    {
                        await this.rabbitMQProducer.Publish("sale-product-not-found", new ProductResponseMessage
                        {
                            SaleId = saleMessage.Id,
                            Status = SaleStatusResponse.NOT_FOUND_PRODUCT.ToString(),
                            Message = "Produto não encontrado"
                        });
                        this.logger.LogInformation($"Produto de Id não encontrado: ${saleMessage.ProductId}");
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error: {ex.Message}");
            }
        }
        public async Task AddProduct(string message)
        {
            try
            {
                var productMessage = JsonSerializer.Deserialize<string>(message);
                if (productMessage != null)
                {
                    this.logger.LogInformation($"Ocorrência: {productMessage}");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error: {ex.Message}");
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                this.logger.LogInformation("Iniciando conexão com o RabbitMQ...");
                var connection = await this.factory.CreateConnectionAsync();
                var consumer = new RabbitMQConsumer(connection);
                consumer.RegisterHandler("validate-product", async message => { await this.ValidateProduct(message); });
                consumer.RegisterHandler("add-product", async message => { await this.AddProduct(message); });
                await consumer.Start();
                this.logger.LogInformation("📡 Consumer de Produtos do RabbitMQ iniciado e aguardando mensagens...");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
    }
}