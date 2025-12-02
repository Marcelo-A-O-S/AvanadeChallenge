using Microsoft.AspNetCore.Mvc;
using StockService.Application.DTOs.Requests;
using StockService.Application.Interfaces;
using StockService.Domain.Entities;
using StockService.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

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
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Obtém uma lista inteira ou paginada de produtos",
            Description = "Retorna uma lista inteira ou paginada de produtos com todas as suas informações."
        )]
        [SwaggerResponse(200, "Lista de produtos retornada com sucesso.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> List([FromQuery] int? page)
        {
            var products = new List<Product>();
            if (page.HasValue)
            {
                products = await this.productServices.List(page ?? 1);
            }
            else
            {
                products = await this.productServices.List();
            }
            return Ok(products);
        }
        [HttpGet("GetProductsWithStock")]
        [Authorize(Roles = "Administrador,Client")]
        public async Task<IActionResult> GetProductsWithStock([FromQuery] int? page)
        {
            var products = new List<Product>();
            if (page.HasValue)
            {
                products = await this.productServices.GetProductsWithStock(page ?? 1);
            }
            else
            {
                products = await this.productServices.GetProductsWithStock();
            }
            return Ok(products);
        }
        [HttpGet("ByIds")]
        [Authorize(Roles = "Administrador,Client")]
        public async Task<IActionResult> GetByIds([FromQuery] List<long> ids)
        {
            var products = await this.productServices.GetByIds(ids);
            return Ok(products);
        }
        [HttpGet("{Id}")]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Obtém um produto específico",
            Description = "Retorna as informações completas de um produto com base no seu identificador único."
        )]
        [SwaggerResponse(200, "Produto encontrado com sucesso.")]
        [SwaggerResponse(404, "Produto não encontrado.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var product = await this.productServices.GetById(Id);
            if (product == null)
                return NotFound(new { message = "Produto não encontrado."});
            return Ok(product);
        }
        [HttpGet("GetQuantity")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetQuantity()
        {
            var productCount = await this.productServices.GetQuantity();
            return Ok(productCount);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [SwaggerOperation(
            Summary = "Cria um novo produto",
            Description = "Adiciona um novo produto no banco de dados e retorna os dados do produto criado."
        )]
        [SwaggerResponse(201, "Produto criado com sucesso.")]
        [SwaggerResponse(400, "Erros de validação")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> Save(ProductRequest productRequest)
        {
            if (ModelState.IsValid)
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
                await this.rabbitMQProducer.Publish("add-product", "Produto salvo com sucesso");
                return CreatedAtAction(nameof(GetById), new { Id = product.Id }, product);
            }
            var erros = ModelState.Values.Select(x => x.Errors);
            return BadRequest(erros);
        }
        [HttpPut("{Id}")]
        [Authorize(Roles = "Administrador")]
        [SwaggerOperation(
            Summary = "Atualiza um produto",
            Description = "Busca e atualiza o produto retornando os dados do produto atualizado."
        )]
        [SwaggerResponse(201, "Produto criado com sucesso.")]
        [SwaggerResponse(400, "Erros de validação")]
        [SwaggerResponse(404, "Produto não encontrado.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> Update([FromRoute] int Id, ProductRequest productRequest)
        {
            if (ModelState.IsValid)
            {
                var productDb = await this.productServices.GetById(Id);
                if (productDb == null)
                    return NotFound(new { message = "Produto não encontrado."});
                productDb.Description = productRequest.Description;
                productDb.Name = productRequest.Name;
                productDb.Price = productRequest.Price;
                productDb.Quantity = productRequest.Quantity;
                productDb.MinimunStock = productRequest.MinimunStock;
                await this.productServices.Update(productDb);
                return Ok(new { message = "Produto atualizado com sucesso!"});
            }
            var erros = ModelState.Values.Select(x => x.Errors);
            return BadRequest(erros);
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Administrador")]
        [SwaggerOperation(
            Summary = "Deleta um produto por Id",
            Description = "Busca e deleta o produto do banco de dados."
        )]
        [SwaggerResponse(200, "Produto deletado com sucesso.")]
        [SwaggerResponse(404, "Produto não encontrado.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> DeleteById([FromQuery] int Id)
        {
            var productDb = await this.productServices.GetById(Id);
            if (productDb == null)
                return NotFound();
            await this.productServices.Delete(productDb);
            return Ok(new { message = "Registro deletado com sucesso!"});
        }
    }
}