using System.Text.Json;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SaleService.Domain.Interfaces;
using SaleService.Infrastructure.Workers;
using SaleService.Infrastructure.Messaging.Contracts;
using SaleService.Domain.Enums;
using SaleService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SaleService.Infrastructure.Messaging.Consumers
{
    public class OrderConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ILogger<OrderConsumer> logger;
        public OrderConsumer(
            IConnectionFactory _factory,
            IServiceScopeFactory _scopeFactory,
            IRabbitMQProducer _rabbitMQProducer,
            ILogger<OrderConsumer> _logger
            )
        {
            this.factory = _factory;
            this.scopeFactory = _scopeFactory;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.logger = _logger;
        }
        public async Task Canceled(string message)
        {
            try
            {
                using var scope = this.scopeFactory.CreateScope();
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                var stockMessage = JsonSerializer.Deserialize<StockResponseMessage>(message);
                if (stockMessage != null)
                {
                    var order = await orderRepository.GetById(stockMessage.OrderId);
                    if (order != null)
                    {
                        order.Status = OrderStatus.REJECT;
                        await orderRepository.Update(order);
                    }
                    else
                    {
                        this.logger.LogWarning("Pedido não encontrado");
                    }

                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error: {ex.Message}");
            }
        }
        public async Task PartiallyCompleted(string message)
        {
            try
            {
                using var scope = this.scopeFactory.CreateScope();
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var stockMessage = JsonSerializer.Deserialize<StockResponseMessage>(message);
                if (stockMessage != null)
                {
                    var order = await orderRepository.GetById(stockMessage.OrderId);
                    if (order != null)
                    {
                        this.logger.LogInformation($"Pedido parcialmente processado com Identificador: ${order.Id}");
                        order.Status = OrderStatus.PARTIALLY_CONFIRMED;
                        await orderRepository.Update(order);
                    }
                    else
                    {
                        this.logger.LogWarning("Pedido não encontrado");
                    }

                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error: {ex.Message}");
            }
        }
        public async Task Completed(string message)
        {
            try
            {
                using var scope = this.scopeFactory.CreateScope();
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var stockMessage = JsonSerializer.Deserialize<StockResponseMessage>(message);
                if (stockMessage != null)
                {
                    var order = await orderRepository.GetById(stockMessage.OrderId);
                    if (order != null)
                    {
                        this.logger.LogInformation($"Pedido processado com sucesso com o Id: ${order.Id}");
                        order.Status = OrderStatus.CONFIRMED;
                        await orderRepository.Update(order);
                    }
                    else
                    {
                        this.logger.LogWarning("Pedido não encontrado");
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error: {ex.Message}");
            }
        }
        public async Task OrderReversalCompleted(string message)
        {
            try
            {
                using var scope = this.scopeFactory.CreateScope();
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var stockMessage = JsonSerializer.Deserialize<StockResponseMessage>(message);
                if (stockMessage != null)
                {
                    var order = await orderRepository.GetById(stockMessage.OrderId);
                    if (order != null)
                    {
                        this.logger.LogInformation($"Pedido rejeitado com o Id: ${order.Id}");
                        order.Status = OrderStatus.REJECT;
                        await orderRepository.Update(order);
                    }
                    else
                    {
                        this.logger.LogWarning("Pedido não encontrado");
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error: {ex.Message}");
            }
        }
        public async Task OrderProductNotFoundOrNotStock(string message)
        {
            try
            {
                using var scope = this.scopeFactory.CreateScope();
                var saleRepository = scope.ServiceProvider.GetRequiredService<ISaleRepository>();
                var stockMessage = JsonSerializer.Deserialize<StockResponseMessage>(message);
                if (stockMessage != null)
                {
                    var sale = await saleRepository.GetById(stockMessage.SaleId);
                    if (sale != null)
                    {
                        if(stockMessage.Status == OrderStatus.NOT_FOUND_PRODUCT.ToString())
                        {
                            this.logger.LogInformation($"Pedido com produto não encontrado com o Id: ${stockMessage.OrderId}");
                            sale.Status = SaleStatus.NOT_FOUND_PRODUCT;
                        }
                        else
                        {
                            this.logger.LogInformation($"Pedido com produto sem estoque com o Id: ${stockMessage.OrderId}");
                            sale.Status = SaleStatus.NOT_STOCK;
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
        public async Task OrderSaleValidate(string message)
        {
            try
            {
                using var scope = this.scopeFactory.CreateScope();
                var saleRepository = scope.ServiceProvider.GetRequiredService<ISaleRepository>();
                
                var stockMessage = JsonSerializer.Deserialize<StockResponseMessage>(message);
                if (stockMessage != null)
                {
                    var sale = await saleRepository.GetById(stockMessage.SaleId);
                    if (sale != null)
                    {
                        this.logger.LogInformation($"Pedido processado com sucesso o Id: ${stockMessage.OrderId}");
                        sale.Status = SaleStatus.CONFIRMED;
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
                consumer.RegisterHandler("order-canceled", async message => { await this.Canceled(message); });
                consumer.RegisterHandler("order-partially-completed", async message => { await this.PartiallyCompleted(message); });
                consumer.RegisterHandler("order-completed", async message => { await this.Completed(message); });
                consumer.RegisterHandler("order-not-found", async message => { await this.Canceled(message); });
                consumer.RegisterHandler("order-product-not-found", async message => { await this.OrderProductNotFoundOrNotStock(message); });
                consumer.RegisterHandler("order-product-not-stock", async message => { await this.OrderProductNotFoundOrNotStock(message); });
                consumer.RegisterHandler("order-sale-valid", async message => { await this.OrderSaleValidate(message); });
                consumer.RegisterHandler("order-reversal-completed", async message => { await this.OrderReversalCompleted(message); });
                await consumer.Start();
                this.logger.LogInformation("📡 Consumer de Pedidos do RabbitMQ iniciado e aguardando mensagens...");
            }catch(Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
    }
}