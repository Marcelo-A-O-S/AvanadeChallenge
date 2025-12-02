using System.Text.Json;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using StockService.Domain.Interfaces;
using StockService.Infrastructure.Workers;
using StockService.Infrastructure.Messaging.Contracts;
using StockService.Domain.Entities;
using StockService.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace StockService.Infrastructure.Messaging.Consumers
{
    public class StockMovementConsumer : BackgroundService
    {
        private readonly IConnectionFactory factory;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly ILogger<StockMovementConsumer> logger;
        public StockMovementConsumer(
           IConnectionFactory _factory,
           IRabbitMQProducer _rabbitMQProducer,
           IServiceScopeFactory _scopeFactory,
           ILogger<StockMovementConsumer> _logger)
        {
            this.factory = _factory;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.scopeFactory = _scopeFactory;
            this.logger = _logger;
        }
        public async Task RegisterMovement(string message)
        {
            try
            {
                using var scope = this.scopeFactory.CreateScope();
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                var stockMovementRepository = scope.ServiceProvider.GetRequiredService<IStockMovementRepository>();
                var order = JsonSerializer.Deserialize<OrderMessage>(message);
                if (order != null)
                {
                    var invalidSale = new List<long>();
                    foreach (var sale in order.Sales)
                    {
                        var product = await productRepository.GetById(sale.ProductId);
                        if (product == null)
                        {
                            await this.rabbitMQProducer.Publish("order-product-not-found", new StockResponseMessage
                            {
                                OrderId = order.Id,
                                SaleId = sale.Id,
                                Status = OrderStatusResponse.NOT_FOUND_PRODUCT.ToString(),
                                Message = "Produto não encontrado"
                            });
                            invalidSale.Add(sale.ProductId);
                            this.logger.LogInformation($"Produto de Id não encontrado: ${sale.ProductId}");
                            continue;
                        }
                        if (product.Quantity < sale.Quantity)
                        {
                            await this.rabbitMQProducer.Publish("order-product-not-stock", new StockResponseMessage
                            {
                                OrderId = order.Id,
                                SaleId = sale.Id,
                                Status = OrderStatusResponse.NOT_STOCK.ToString(),
                                Message = "Não há estoque suficiente"
                            });
                            invalidSale.Add(product.Id);
                            this.logger.LogInformation($"Não há estoque suficiente para o Id do produto: {product.Id}");
                            continue;
                        }
                        var stockMovement = new StockMovement
                        {
                            Id = 0,
                            OrderId = order.Id,
                            SaleId = sale.Id,
                            ProductId = sale.ProductId,
                            Quantity = sale.Quantity,
                            Type = TypeMovement.Output,
                            Reason = ReasonMovement.Sale
                        };
                        stockMovement.Validate();
                        product.UpdateQuantity(sale.Quantity, stockMovement.Type);
                        await stockMovementRepository.Save(stockMovement);
                        this.logger.LogInformation("Movimento de estoque salvo com sucesso");
                        await productRepository.Update(product);
                        this.logger.LogInformation("Produto atualizado com sucesso");
                        await this.rabbitMQProducer.Publish("order-sale-valid", new StockResponseMessage
                        {
                            SaleId = sale.Id,
                            Status = OrderStatusResponse.CONFIRMED.ToString(),
                            Message = "Venda realizada com sucesso"
                        });
                        this.logger.LogInformation("Venda validada com sucesso!");
                    }
                    if (invalidSale.Count == order.Sales.Count)
                    {
                        await this.rabbitMQProducer.Publish("order-canceled", new StockResponseMessage
                        {
                            OrderId = order.Id,
                            Status = OrderStatusResponse.REJECT.ToString(),
                            Message = "Não foi possivel processar todos os produtos!"
                        });
                        this.logger.LogInformation($"Não foi possivel processar os produtos do pedido de Id: {order.Id}");
                        return;
                    }
                    if (invalidSale.Any())
                    {
                        await this.rabbitMQProducer.Publish("order-partially-completed", new StockResponseMessage
                        {
                            OrderId = order.Id,
                            Status = OrderStatusResponse.PARTIALLY_CONFIRMED.ToString(),
                            Message = "Pedido parcialmente processado"
                        });
                        this.logger.LogInformation($"Produtos parcialmente processados do pedido de Id: {order.Id}");
                        return;
                    }
                    await this.rabbitMQProducer.Publish("order-completed", new StockResponseMessage
                    {
                        OrderId = order.Id,
                        Status = OrderStatusResponse.CONFIRMED.ToString(),
                        Message = "Pedido realizado com sucesso!"
                    });
                    this.logger.LogInformation("Pedido realizado com sucesso!");
                }
            }
            catch (Exception ex)
            {

                this.logger.LogError($"Error: {ex.Message} ");
            }
        }
        public async Task AddStockMovement(string message)
        {
            try
            {
                var stockMessage = JsonSerializer.Deserialize<string>(message);
                if (stockMessage != null)
                {
                    this.logger.LogInformation($"Ocorrência: {stockMessage}");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error: {ex.Message}");
            }
        }
        public async Task ReversalStockMovement(string message)
        {
            try
            {
                using var scope = this.scopeFactory.CreateScope();
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                var stockMovementRepository = scope.ServiceProvider.GetRequiredService<IStockMovementRepository>();
                var order = JsonSerializer.Deserialize<OrderMessage>(message);
                var stockMovements = await stockMovementRepository.GetAllByOrderId(order.Id);
                if (stockMovements.Count == 0)
                {
                    await this.rabbitMQProducer.Publish("order-not-found", new StockResponseMessage
                    {
                        OrderId = order.Id,
                        Message = "Pedido não encontrado"
                    });
                    return;
                }
                foreach (var stock in stockMovements)
                {
                    var product = await productRepository.GetById(stock.ProductId);
                    if (product == null)
                    {
                        await this.rabbitMQProducer.Publish("order-product-not-found", new StockResponseMessage
                        {
                            OrderId = order.Id,
                            SaleId = stock.SaleId,
                            Message = "Produto não encontrado"
                        });
                        continue;
                    }
                    stock.Type = TypeMovement.Input;
                    stock.Reason = ReasonMovement.Reversal;
                    product.AmountReversal(stock.Quantity, stock.Type);
                    await stockMovementRepository.Update(stock);
                    await productRepository.Update(product);
                    await this.rabbitMQProducer.Publish("reversal-sale", new StockResponseMessage
                    {
                        OrderId = order.Id,
                        SaleId = stock.SaleId,
                        Message = "Venda estornada com sucesso"
                    });
                }
                await this.rabbitMQProducer.Publish("order-reversal-completed", new StockResponseMessage
                {
                    OrderId = order.Id,
                    Message = "Estorno realizado com sucesso"
                });
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
                consumer.RegisterHandler("add-order", async message => { await this.RegisterMovement(message); });
                consumer.RegisterHandler("add-stock-movement", async message => { await this.AddStockMovement(message); });
                consumer.RegisterHandler("stock-movement-reversal", async message => { await this.ReversalStockMovement(message); });
                await consumer.Start();
                this.logger.LogInformation("📡 Consumer da Movimentação de Estoque do RabbitMQ iniciado e aguardando mensagens...");
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Erro ao conectar ao RabbitMQ: {ex.Message}");
            }
        }
    }
}