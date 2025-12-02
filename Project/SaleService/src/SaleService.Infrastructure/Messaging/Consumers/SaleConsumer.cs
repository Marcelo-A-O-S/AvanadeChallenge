using System.Text.Json;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SaleService.Domain.Interfaces;
using SaleService.Infrastructure.Workers;
using Microsoft.Extensions.DependencyInjection;
using SaleService.Infrastructure.Messaging.Contracts;
using SaleService.Domain.Enums;
using SaleService.Domain.Entities;
using Microsoft.Extensions.Logging;


namespace SaleService.Infrastructure.Messaging.Consumers
{
    public class SaleConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<SaleConsumer> logger;
        public SaleConsumer(
            IConnectionFactory _factory,
            IServiceScopeFactory _scopeFactory,
            ILogger<SaleConsumer> _logger
        )
        {
            this.factory = _factory;
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        public async Task ValidateSale(string message)
        {
            try
            {
                using var scope = this.scopeFactory.CreateScope();
                var saleRepository = scope.ServiceProvider.GetRequiredService<ISaleRepository>();
                
                var saleMessage = JsonSerializer.Deserialize<SaleResponseMessage>(message);
                if (saleMessage != null)
                {
                    var sale = await saleRepository.GetById(saleMessage.SaleId);
                    if (sale != null)
                    {
                        this.logger.LogInformation($"Venda parcialmente processada com Identificador: ${sale.Id}");
                        sale.Status = SaleStatus.PARTIALLY_CONFIRMED;
                        await saleRepository.Update(sale);
                    }
                    else
                    {
                        this.logger.LogWarning("Venda não encontrada");
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error: {ex.Message}");
            }
        }
        public async Task ProductNotFoundOrNotStock(string message)
        {
            try
            {
                using var scope = this.scopeFactory.CreateScope();
                var saleRepository = scope.ServiceProvider.GetRequiredService<ISaleRepository>();
                var saleMessage = JsonSerializer.Deserialize<SaleResponseMessage>(message);
                if (saleMessage != null)
                {
                    var sale = await saleRepository.GetById(saleMessage.SaleId);
                    if (sale != null)
                    {
                        if(saleMessage.Status == SaleStatus.NOT_FOUND_PRODUCT.ToString())
                        {
                            sale.Status = SaleStatus.NOT_FOUND_PRODUCT;
                            this.logger.LogInformation($"Venda com o produto não encontrado de Id de venda: ${sale.Id}");
                        }
                        else
                        {
                            sale.Status = SaleStatus.NOT_STOCK;
                            this.logger.LogInformation($"Venda com o produto sem estoque de Id de venda: ${sale.Id}");
                        }
                        await saleRepository.Update(sale);
                    }
                    else
                    {
                        this.logger.LogWarning("Venda não encontrada");
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error: {ex.Message}");
            }
        }
        public async Task AddSale(string message)
        {
            try
            {
                var saleMessage = JsonSerializer.Deserialize<string>(message);
                if (saleMessage != null)
                {
                    this.logger.LogInformation($"Ocorrência: {saleMessage}");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error: {ex.Message}");
            }
        }
        public async Task ReversalSale(string message)
        {
            try
            {
                using var scope = this.scopeFactory.CreateScope();
                var saleRepository = scope.ServiceProvider.GetRequiredService<ISaleRepository>();
                var saleMessage = JsonSerializer.Deserialize<SaleResponseMessage>(message);
                if (saleMessage != null)
                {
                    var sale = await saleRepository.GetById(saleMessage.SaleId);
                    if (sale != null)
                    {
                        this.logger.LogInformation($"Venda rejeitada com Identificador: ${sale.Id}");
                        sale.Status = SaleStatus.REJECT;
                        await saleRepository.Update(sale);
                    }
                    else
                    {
                        this.logger.LogWarning("Venda não encontrada");
                    }
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
                consumer.RegisterHandler("add-sale", async message => { await this.AddSale(message); });
                consumer.RegisterHandler("sale-product-not-found", async message => { await this.ProductNotFoundOrNotStock(message); });
                consumer.RegisterHandler("sale-not-stock", async message => { await this.ProductNotFoundOrNotStock(message); });
                consumer.RegisterHandler("sale-valid", async message => { await this.ValidateSale(message); });
                consumer.RegisterHandler("reversal-sale", async message => { await this.ReversalSale(message); });
                await consumer.Start();
                this.logger.LogInformation("📡 Consumer de Vendas do RabbitMQ iniciado e aguardando mensagens...");
            }catch(Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
    }
}