using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockService.Application.DTOs.Requests;
using StockService.Application.Interfaces;
using StockService.Domain.Entities;
using StockService.Domain.Enums;
using StockService.Domain.Interfaces;
using StockService.Infrastructure.Messaging.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace StockService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockMovementServices stockMovementServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        private readonly IProductServices productServices;
        public StockController(IStockMovementServices _stockMovementServices, IRabbitMQProducer _rabbitMQProducer, IProductServices _productServices)
        {
            this.stockMovementServices = _stockMovementServices;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.productServices = _productServices;
        }
        [HttpGet]
        [SwaggerOperation(
            Summary = "Obtém uma lista inteira ou paginada de movimentações de estoque",
            Description = "Retorna uma lista inteira ou paginada de movimentações de estoque com todas as suas informações."
        )]
        [SwaggerResponse(200, "Lista de movimentações do estoque retornada com sucesso.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> List([FromQuery] int? page)
        {
            var stocks = new List<StockMovement>();
            if (page != null)
            {
                stocks = await this.stockMovementServices.List(page ?? 1);
            }
            else
            {
                stocks = await this.stockMovementServices.List();
            }
            return Ok(stocks);
        }
        [HttpGet("{Id}")]
        [SwaggerOperation(
            Summary = "Obtém uma movimentação de estoque em específico",
            Description = "Retorna as informações completas de uma movimentação de estoque com base no seu identificador único."
        )]
        [SwaggerResponse(200, "Movimentação de estoque encontrada com sucesso.")]
        [SwaggerResponse(404, "Movimentação de estoque não encontrado.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var stock = await this.stockMovementServices.GetById(Id);
            if (stock == null)
                return NotFound(new { message = "Movimentação de estoque não encontrada."});
            return Ok(stock);
        }
        [HttpGet("GetQuantity")]
        [Authorize(Roles="Administrador")]
        public async Task<IActionResult> GetQuantity()
        {
            var stockCount = await this.stockMovementServices.GetQuantity();
            return Ok(stockCount);
        }
        [HttpPost]
        [SwaggerOperation(
            Summary = "Adiciona uma nova movimentação de estoque",
            Description = "Cria um novo registro de uma movimentação de estoque."
        )]
        [SwaggerResponse(201, "Movimentação de estoque registrada com sucesso.")]
        [SwaggerResponse(400, "Erros de validação")]
        [SwaggerResponse(404, "Produto relacionado a movimentação de estoque não encontrado.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> Save(StockMovementRequest movementRequest)
        {
            if (ModelState.IsValid)
            {
                var product = await this.productServices.GetById(movementRequest.ProductId);
                if (product == null)
                {
                    return NotFound(new { message = "Produto não encontrado."});
                }
                var stock = new StockMovement
                {
                    Id = 0,
                    OrderId = movementRequest.OrderId,
                    SaleId = movementRequest.SaleId,
                    ProductId = product.Id,
                    Quantity = movementRequest.Quantity,
                    Reason = movementRequest.Reason,
                    Type = movementRequest.Type
                };
                product.UpdateQuantity(movementRequest.Quantity, movementRequest.Type);
                await this.stockMovementServices.Save(stock);
                await this.rabbitMQProducer.Publish("add-stock-movement", "Movimento de estoque realizado com sucesso");
                return CreatedAtAction(nameof(GetById), new { Id = stock.Id }, stock);
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpPost("add-movement-order")]
        [SwaggerOperation(
            Summary = "Adiciona uma nova movimentação de estoque através de um pedido",
            Description = "Cria um novo registro de uma movimentação de estoque com dados de requisição de um pedido."
        )]
        [SwaggerResponse(201, "Movimentação de estoque registrada com sucesso.")]
        [SwaggerResponse(400, "Erros de validação")]
        [SwaggerResponse(400, "Não foi possivel processar todos os pedidos")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> AddMovementOrder(OrderRequest orderRequest)
        {
            if (ModelState.IsValid)
            {
                var invalidSale = new List<long>();
                foreach (var sale in orderRequest.Sales)
                {
                    var product = await productServices.GetById(sale.ProductId);
                    if (product == null)
                    {
                        await this.rabbitMQProducer.Publish("order-product-not-found", new StockResponseMessage
                        {
                            OrderId = orderRequest.Id,
                            SaleId = sale.Id,
                            Status = SaleStatusResponse.REJECT.ToString(),
                            Message = "Produto não encontrado"
                        });
                        invalidSale.Add(sale.ProductId);
                        continue;
                    }
                    if (product.Quantity < sale.Quantity)
                    {
                        await this.rabbitMQProducer.Publish("order-product-not-stock", new StockResponseMessage
                        {
                            OrderId = orderRequest.Id,
                            SaleId = sale.Id,
                            Status = SaleStatusResponse.REJECT.ToString(),
                            Message = "Não há estoque suficiente"
                        });
                        invalidSale.Add(product.Id);
                        continue;
                    }
                    var stockMovement = new StockMovement
                    {
                        Id = 0,
                        OrderId = orderRequest.Id,
                        SaleId = sale.Id,
                        ProductId = sale.ProductId,
                        Quantity = sale.Quantity,
                        Type = TypeMovement.Output,
                        Reason = ReasonMovement.Sale
                    };
                    stockMovement.Validate();
                    product.UpdateQuantity(sale.Quantity, stockMovement.Type);
                    await stockMovementServices.Save(stockMovement);
                    await productServices.Save(product);
                    await this.rabbitMQProducer.Publish("order-sale-valid", new StockResponseMessage
                    {
                        SaleId = sale.Id,
                        Status = SaleStatusResponse.CONFIRMED.ToString(),
                        Message = "Venda realizada com sucesso"
                    });
                }
                if (invalidSale.Count == orderRequest.Sales.Count)
                {
                    await this.rabbitMQProducer.Publish("order-canceled", new StockResponseMessage
                    {
                        OrderId = orderRequest.Id,
                        Status = OrderStatusResponse.REJECT.ToString(),
                        Message = "Não foi possivel processar todos os produtos!"
                    });
                    return BadRequest(new { message = "Não foi possivel processar todos os produtos."});
                }
                if (invalidSale.Any())
                {
                    await this.rabbitMQProducer.Publish("order-partially-completed", new StockResponseMessage
                    {
                        OrderId = orderRequest.Id,
                        Status = OrderStatusResponse.PARTIALLY_CONFIRMED.ToString(),
                        Message = "Pedido parcialmente processado"
                    });
                    return BadRequest(new { message = "Pedido parcialmente processado."});
                }
                await this.rabbitMQProducer.Publish("order-completed", new StockResponseMessage
                {
                    OrderId = orderRequest.Id,
                    Status = OrderStatusResponse.CONFIRMED.ToString(),
                    Message = "Pedido realizado com sucesso!"
                });
                return Ok(new { message = "Movimento de estoque registrado e atualizado com sucesso!"});
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpPut("reversal-stock-movement/{orderId}")]
        public async Task<IActionResult> ReversalOfStockMovement([FromRoute] int orderId)
        {
            var stockMovements = await this.stockMovementServices.GetAllByOrderId(orderId);
            if (stockMovements.Count == 0)
            {
                await this.rabbitMQProducer.Publish("order-not-found", new
                {
                    OrderId = orderId,
                    Message = "Nenhum movimento de estoque encontrado para este pedido."
                });
                return NotFound(new { message = "Nenhum movimento de estoque encontrado para este pedido."});
            }
            var reversed = new List<long>();
            foreach (var stock in stockMovements)
            {
                var product = await this.productServices.GetById(stock.ProductId);
                if (product == null)
                {
                    await this.rabbitMQProducer.Publish("order-product-not-found", new
                    {
                        OrderId = stock.OrderId,
                        ProductId = stock.ProductId,
                        Message = "Produto não encontrado para estorno."
                    });
                    continue;
                }

                product.AmountReversal(stock.Quantity, stock.Type);
                stock.Type = TypeMovement.Input;
                stock.Reason = ReasonMovement.Reversal;
                await this.stockMovementServices.Update(stock);
                await this.productServices.Update(product);
                await this.rabbitMQProducer.Publish("reversal-sale", new
                {
                    OrderId = stock.OrderId,
                    ProductId = stock.ProductId,
                    Message = "Venda revertida e estoque restaurado com sucesso."
                });
                reversed.Add(stock.Id);
            }
            await this.rabbitMQProducer.Publish("reversal-order-completed", new
            {
                OrderId = orderId,
                MovementsReversed = reversed.Count,
                Message = "Todos os movimentos de estoque foram revertidos com sucesso."
            });
            return Ok(new { message = $"Estorno concluído para {reversed.Count} movimentos de estoque."});
        }
    }
}