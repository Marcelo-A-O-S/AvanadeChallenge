using Microsoft.AspNetCore.Mvc;
using SaleService.Application.DTOs.Requests;
using SaleService.Application.Interfaces;
using SaleService.Domain.Entities;
using SaleService.Domain.Interfaces;

namespace SaleService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleServices saleServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public SalesController(ISaleServices _saleServices, IRabbitMQProducer _rabbitMQProducer)
        {
            this.saleServices = _saleServices;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var sales = await this.saleServices.List();
            return Ok(sales);
        }
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int page)
        {
            var sales = await this.saleServices.List(page);
            return Ok(sales);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var sale = await this.saleServices.GetById(Id);
            if (sale == null)
                return NotFound();
            return Ok(sale);
        }
        [HttpPost]
        public async Task<IActionResult> Save(SaleRequest saleRequest)
        {
            var sale = new Sale
            {
                Id = 0,
                ProductId = saleRequest.ProductId,
                Quantity = saleRequest.Quantity,
                UnitPrice = saleRequest.UnitPrice
            };
            await this.saleServices.Save(sale);
            await this.rabbitMQProducer.Publish(eventName: "add-sale", data: new { saleId = sale.Id });
            return Ok(sale);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update([FromRoute] int Id, SaleRequest saleRequest)
        {
            var saleDb = await this.saleServices.GetById(Id);
            if (saleDb == null)
                return NotFound();
            var sale = new Sale
            {
                Id = Id,
                ProductId = saleRequest.ProductId,
                Quantity = saleRequest.Quantity,
                UnitPrice = saleRequest.UnitPrice
            };
            await this.saleServices.Update(sale);
            await this.rabbitMQProducer.Publish(eventName: "update-sale", data: new { saleId = sale.Id });
            return Ok(sale);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteByIdInRoute([FromRoute]int Id)
        {
            try
            {
                var sale = await this.saleServices.GetById(Id);
                if (sale == null)
                    return NotFound();

                await this.saleServices.Delete(sale);
                await this.rabbitMQProducer.Publish(eventName: "delete-sale", data: new { saleId = sale.Id });
                return Ok("Registro deletado com sucesso!");
            }catch(Exception ex)
            {
                return BadRequest($"Erro no servidor: {ex.Message}");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteById([FromQuery] int Id)
        {
            try
            {
                var sale = await this.saleServices.GetById(Id);
                if (sale == null)
                    return NotFound();

                await this.saleServices.Delete(sale);
                await this.rabbitMQProducer.Publish(eventName: "delete-sale", data: new { saleId = sale.Id });
                return Ok("Registro deletado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro no servidor: {ex.Message}");
            }
        }
    }
}