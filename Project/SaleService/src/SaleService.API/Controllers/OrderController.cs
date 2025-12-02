using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleService.Application.DTOs.Message;
using SaleService.Application.DTOs.Requests;
using SaleService.Application.DTOs.Responses;
using SaleService.Application.Interfaces;
using SaleService.Domain.Entities;
using SaleService.Domain.Enums;
using SaleService.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SaleService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ISaleServices saleServices;
        private readonly IOrderServices orderServices;
        
        private readonly IRabbitMQProducer rabbitMQProducer;
        public OrderController(
            IOrderServices _orderServices,
            IRabbitMQProducer _rabbitMQProducer,
            ISaleServices _saleServices)
        {
            this.orderServices = _orderServices;
            this.rabbitMQProducer = _rabbitMQProducer;
            this.saleServices = _saleServices;
        }
        [HttpGet]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Obtém uma lista inteira ou paginada de pedidos",
            Description = "Retorna uma lista inteira ou paginada de pedidos com todas as suas informações."
        )]
        [SwaggerResponse(200, "Lista de pedidos retornada com sucesso.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> List([FromQuery] int? page)
        {
            var orders = new List<Order>();
            if (page.HasValue)
            {
                orders = await this.orderServices.List(page ?? 1);
            }
            else
            {
                orders = await this.orderServices.List();
            }
            return Ok(orders);
        }
        [HttpGet("{Id}")]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Obtém uma pedido específica",
            Description = "Retorna as informações completas de um pedido com base no seu identificador único."
        )]
        [SwaggerResponse(200, "Pedido encontrado com sucesso.")]
        [SwaggerResponse(404, "Pedido não encontrado.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var order = await this.orderServices.GetById(Id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }
        [HttpGet("{Id}/Status")]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Mostra o status de um pedido especifico",
            Description = "Retorna as informações completas de um pedido com base no seu identificador único."
        )]
        [SwaggerResponse(200, "Pedido encontrado com sucesso.")]
        [SwaggerResponse(404, "Pedido não encontrado.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> GetStatusById([FromRoute] int Id)
        {
            var order = await this.orderServices.GetById(Id);
            if (order == null)
                return NotFound(new { message = "Pedido não encontrado" });
            if (order.Status == OrderStatus.PROCESSING)
                return Ok(new { message = "Pedido sendo processado..." });
            if (order.Status == OrderStatus.PARTIALLY_CONFIRMED || order.Status == OrderStatus.REJECT)
            {
                var salesErrors = order.Sales.FindAll(s => s.Status != SaleStatus.CONFIRMED || s.Status != SaleStatus.PROCESSING).ToList();
                Ok(new { status = order.Status.ToString(), errors = salesErrors });
            }
            return Ok(order);
        }
        [HttpGet("GetQuantity")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetQuantity()
        {
            var ordersCount = await this.orderServices.GetQuantity();
            return Ok(ordersCount);
        }
        [HttpGet("GetQuantityByUserId")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetQuantityByUserId([FromQuery] long userId)
        {
            var ordersCount = await this.orderServices.GetQuantityByUserId(userId);
            return Ok(ordersCount);
        }
        [HttpGet("GetAllByUserId")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetAllByUserId([FromQuery] long userId,[FromQuery] int page = 1, int itemsPage = 10)
        {
            var orders = await this.orderServices.GetAllByUserId(userId,page,itemsPage);
            var orderResponse = orders.Select(o=> new OrderResponse
            {
                Id = o.Id,
                UserId = o.UserId,
                Status = o.Status.ToString(),
                CreatedAt = o.CreatedAt,
                Sales = o.Sales.Select(s=> new SaleResponse
                {
                    ProductId = s.ProductId,
                    Quantity = s.Quantity,
                    UnitPrice = s.UnitPrice,
                    UserId = s.UserId
                }).ToList()   
            }).ToList();
            return Ok(orderResponse);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Registrar pedido",
            Description = "Registra um pedido no banco de dados e retorna os dados do pedido criado."
        )]
        [SwaggerResponse(201, "Pedido criado com sucesso.")]
        [SwaggerResponse(400, "Erros de validação")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> Save(OrderRequest orderRequest)
        {
            if (ModelState.IsValid)
            {
                var order = new Order
                {
                    Id = 0,
                    UserId = orderRequest.UserId,
                    CreatedAt = DateTime.UtcNow,
                    Status = OrderStatus.PROCESSING,
                    UpdatedAt = DateTime.UtcNow,
                    Sales = new List<Sale>()
                };
                await this.orderServices.Save(order);
                foreach (var sale in orderRequest.Sales)
                {
                    sale.OrderId = order.Id;
                    await this.saleServices.Update(sale);
                }
                order.Sales = await this.saleServices.GetByOrderId(order.Id);
                await this.rabbitMQProducer.Publish("add-order", new OrderMessage
                {
                    Id = order.Id,
                    Sales = order.Sales.Select(x =>
                        new SaleMessage
                        {
                            Id = x.Id,
                            ProductId = x.ProductId,
                            Quantity = x.Quantity,
                            UnitPrice = x.UnitPrice,
                            Status = x.Status.ToString()
                        }).ToList(),
                    Status = order.Status.ToString(),
                });
                return Ok(new { message = "Pedido em processamento..."});
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpPut("{Id}")]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Atualiza o registro de um pedido",
            Description = "Busca e atualiza o registro de um pedido no banco de dados."
        )]
        [SwaggerResponse(200, "Pedido atualizada com sucesso.")]
        [SwaggerResponse(400, "Erros de validação")]
        [SwaggerResponse(404, "Pedido não encontrado.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> Update([FromRoute] int Id, OrderRequest orderRequest)
        {
            if (ModelState.IsValid)
            {
                var orderDb = await this.orderServices.GetById(Id);
                if (orderDb == null)
                    return NotFound(new { message = "Pedido não encontrado" });
                if (orderDb.Status == OrderStatus.CONFIRMED)
                {
                    return BadRequest(new { message = "Não é possivel realizar um pedido finalizado!"});
                }
                orderDb.Id = Id;
                orderDb.UserId = orderRequest.UserId;
                orderDb.Sales = orderRequest.Sales;
                orderDb.UpdatedAt = DateTime.UtcNow;
                await this.orderServices.Save(orderDb);
                await this.rabbitMQProducer.Publish("update-order", orderDb);
                return Ok(new { message = "Pedido atualizado com sucesso!" });
            }
            var errors = ModelState.Values.Select(x => x.Errors);
            return BadRequest(errors);
        }
        [HttpDelete("{Id}")]
        [SwaggerOperation(
            Summary = "Deleta um registro de um pedido",
            Description = "Deleta um registro de um pedido no banco de dados."
        )]
        [SwaggerResponse(200, "Pedido deletada com sucesso.")]
        [SwaggerResponse(404, "Pedido não encontrado.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> DeleteByIdInRoute([FromRoute] int Id)
        {
            var order = await this.orderServices.GetById(Id);
            if (order == null)
                return NotFound(new { message = "Pedido não encontrado " });
            await this.orderServices.Delete(order);
            await this.rabbitMQProducer.Publish("delete-order", new { OrderId = Id });
            return Ok(new { message = "Registro deletado com sucesso!" });
        }
    }
}