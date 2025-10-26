using Microsoft.AspNetCore.Mvc;
using SaleService.Application.DTOs.Requests;
using SaleService.Application.Interfaces;
using SaleService.Domain.Entities;
using SaleService.Domain.Interfaces;

namespace SaleService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices orderServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public OrderController(IOrderServices _orderServices, IRabbitMQProducer _rabbitMQProducer)
        {
            this.orderServices = _orderServices;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var orders = await this.orderServices.List();
            return Ok(orders);
        }
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int page)
        {
            var orders = await this.orderServices.List(page);
            return Ok(orders);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var order = await this.orderServices.GetById(Id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }
        [HttpPost]
        public async Task<IActionResult> Save(OrderRequest orderRequest)
        {
            if (orderRequest.UserId == 0)
                return BadRequest("");
            if (orderRequest.Sales.Count == 0)
                return BadRequest("");

            var order = new Order
            {
                Id = 0,
                UserId = orderRequest.UserId,
                Sales = orderRequest.Sales
            };
            await this.orderServices.Save(order);
/*             var orderMessage = new OrderMessage
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    IsCanceled = order.IsCanceled,
                    Sales = order.Sales.Select(s => new SaleMessage
                    {
                        ProductId = s.ProductId,
                        Quantity = s.Quantity,
                        UnitPrice = s.UnitPrice,
                        isValid = s.isValid
                    }).ToList()
                }; */
            await this.rabbitMQProducer.Publish("add-order", order);
            return Ok(order);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update([FromRoute] int Id, OrderRequest orderRequest)
        {
            var orderDb = await this.orderServices.GetById(Id);
            if (orderDb == null)
                return NotFound();
            if (orderRequest.UserId == 0)
                return BadRequest("");
            if (orderRequest.Sales.Count == 0)
                return BadRequest("");
            var order = new Order
            {
                Id = Id,
                UserId = orderRequest.UserId,
                Sales = orderRequest.Sales
            };
            await this.orderServices.Save(order);
            await this.rabbitMQProducer.Publish("update-order", order);
            return Ok(order);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteByIdInRoute([FromRoute] int Id)
        {
            var order = await this.orderServices.GetById(Id);
            if (order == null)
                return NotFound();
            await this.orderServices.Delete(order);
            await this.rabbitMQProducer.Publish("delete-order", new { OrderId = Id });
            return Ok("Registro deletado com sucesso!");
        }
    }
}