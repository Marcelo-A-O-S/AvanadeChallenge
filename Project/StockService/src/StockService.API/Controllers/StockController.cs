using Microsoft.AspNetCore.Mvc;
using StockService.Application.Interfaces;
using StockService.Domain.Interfaces;

namespace StockService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockMovementServices stockMovementServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public StockController(IStockMovementServices _stockMovementServices, IRabbitMQProducer _rabbitMQProducer)
        {
            this.stockMovementServices = _stockMovementServices;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var stocks = await this.stockMovementServices.List();
            return Ok(stocks);
        }
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int page)
        {
            var stocks = await this.stockMovementServices.List(page);
            return Ok(stocks);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var stock = await this.stockMovementServices.GetById(Id);
            if (stock == null)
                return NotFound();
            return Ok(stock);
        }
        [HttpPost]
        public async Task<IActionResult> Save()
        {
            return Ok();
        }
    }
}