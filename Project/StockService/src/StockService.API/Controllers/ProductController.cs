using Microsoft.AspNetCore.Mvc;
using StockService.Application.DTOs.Requests;
using StockService.Application.Interfaces;
using StockService.Domain.Entities;
using StockService.Domain.Interfaces;

namespace StockService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices productServices;
        private readonly IRabbitMQProducer rabbitMQProducer;
        public ProductController(IProductServices _productServices, IRabbitMQProducer _rabbitMQProducer)
        {
            this.productServices = _productServices;
            this.rabbitMQProducer = _rabbitMQProducer;
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var products = await this.productServices.List();
            return Ok(products);
        }
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int page)
        {
            var products = await this.productServices.List(page);
            return Ok(products);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var product = await this.productServices.GetById(Id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> Save(ProductRequest productRequest)
        {
            var product = new Product
            {
                Id = 0,
                Description = productRequest.Description,
                Name = productRequest.Name,
                Price = productRequest.Price,
                Quantity = productRequest.Quantity,
                MinimunStock = productRequest.MinimunStock
            };
            await this.productServices.Save(product);
            return Ok(product);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update([FromRoute] int Id, ProductRequest productRequest)
        {
            var productDb = await this.productServices.GetById(Id);
            if (productDb == null)
                return NotFound();
            var product = new Product
            {
                Id = Id,
                Description = productRequest.Description,
                Name = productRequest.Name,
                Price = productRequest.Price,
                Quantity = productRequest.Quantity,
                MinimunStock = productRequest.MinimunStock
            };
            await this.productServices.Update(product);
            return Ok(product);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteById([FromQuery] int Id)
        {
            var productDb = await this.productServices.GetById(Id);
            if (productDb == null)
                return NotFound();
            await this.productServices.Delete(productDb);
            return Ok("Registro deletado com sucesso!");
        }
    }
}