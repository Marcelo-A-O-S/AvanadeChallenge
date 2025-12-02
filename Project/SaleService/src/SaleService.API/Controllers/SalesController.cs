using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleService.Application.DTOs.Message;
using SaleService.Application.DTOs.Requests;
using SaleService.Application.Interfaces;
using SaleService.Domain.Entities;
using SaleService.Domain.Enums;
using SaleService.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

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
        [HttpGet("GetSalesInProgress")]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Obtém uma lista inteira ou paginada de informações sobre os produtos de vendas de um usuário.",
            Description = "Retorna uma lista inteira ou paginada de informações sobre os produtos de vendas de um usuário."
        )]
        [SwaggerResponse(200, "Lista de vendas retornada com sucesso.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> GetSalesInProgress([FromQuery] int userId, [FromQuery] int page = 1)
        {
            var salesInfoProducts = await this.saleServices.GetSalesInProgress(userId, page);
            return Ok(salesInfoProducts);
        }
        [HttpGet("GetSalesInfoProducts")]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Obtém uma lista inteira ou paginada de informações sobre os produtos de vendas.",
            Description = "Retorna uma lista inteira ou paginada de informações sobre os produtos de vendas."
        )]
        [SwaggerResponse(200, "Lista de vendas retornada com sucesso.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> GetSalesInfoProducts([FromQuery] int productInitial, [FromQuery] int productFinally)
        {
            var salesInfoProducts = await this.saleServices.GetSalesInfoProducts(productInitial, productFinally);
            return Ok(salesInfoProducts);
        }
        [HttpGet("GetSalesInfoProductsByIds")]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Obtém uma lista inteira ou paginada de informações sobre os produtos de vendas.",
            Description = "Retorna uma lista inteira ou paginada de informações sobre os produtos de vendas."
        )]
        [SwaggerResponse(200, "Lista de vendas retornada com sucesso.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> GetSalesInfoProductsByIds([FromQuery(Name = "productsId")] List<long> productsId)
        {
            var salesInfoProducts = await this.saleServices.GetSalesInfoProductsByIds(productsId);
            return Ok(salesInfoProducts);
        }
        [HttpGet("GetSalesInfoProductsConfirmedByIds")]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Obtém uma lista inteira ou paginada de informações sobre os produtos de vendas.",
            Description = "Retorna uma lista inteira ou paginada de informações sobre os produtos de vendas."
        )]
        [SwaggerResponse(200, "Lista de vendas retornada com sucesso.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> GetSalesInfoProductsConfirmedByIds([FromQuery(Name = "productsId")] List<long> productsId)
        {
            var salesInfoProducts = await this.saleServices.GetSalesInfoProductsConfirmedByIds(productsId);
            return Ok(salesInfoProducts);
        }
        [HttpGet]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Obtém uma lista inteira ou paginada de vendas",
            Description = "Retorna uma lista inteira ou paginada de vendas com todas as suas informações."
        )]
        [SwaggerResponse(200, "Lista de vendas retornada com sucesso.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> List([FromQuery] int? page)
        {
            var sales = new List<Sale>();
            if (page.HasValue)
            {
                sales = await this.saleServices.List(page ?? 1);
            }
            else
            {
                sales = await this.saleServices.List();
            }
            return Ok(sales);
        }

        [HttpGet("{Id}")]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Obtém uma venda específica",
            Description = "Retorna as informações completas de uma venda com base no seu identificador único."
        )]
        [SwaggerResponse(200, "Venda encontrado com sucesso.")]
        [SwaggerResponse(404, "Venda não encontrada.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var sale = await this.saleServices.GetById(Id);
            if (sale == null)
                return NotFound();
            return Ok(sale);
        }
        [HttpGet("GetByUserId")]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Busca vendas relacionadas a um usuário.",
            Description = "Busca vendas relacionadas a um usuário e todas suas informações."
        )]
        [SwaggerResponse(200, "Venda encontrado com sucesso.")]
        [SwaggerResponse(404, "Venda não encontrada.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> GetByUserId([FromQuery] int userId, [FromQuery] int page = 1)
        {
            var sales = await this.saleServices.GetByUserId(userId, page);
            return Ok(sales);
        }
        [HttpGet("GetQuantity")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetQuantity()
        {
            var salesCount = await this.saleServices.GetQuantity();
            return Ok(salesCount);
        }
        [HttpGet("GetQuantityInProgressByUserId")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetQuantityInProgressByUserId([FromQuery] long userId)
        {
            var salesCount = await this.saleServices.GetQuantityInProgressByUserId(userId);
            return Ok(salesCount);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Registrar venda",
            Description = "Registra uma venda no banco de dados e retorna os dados da venda criada."
        )]
        [SwaggerResponse(201, "Venda criado com sucesso.")]
        [SwaggerResponse(400, "Erros de validação")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> Save(SaleRequest saleRequest)
        {
            if (ModelState.IsValid)
            {
                var sale = new Sale
                {
                    Id = 0,
                    ProductId = saleRequest.ProductId,
                    UserId = saleRequest.UserId,
                    Quantity = saleRequest.Quantity,
                    UnitPrice = saleRequest.UnitPrice,
                    Status = SaleStatus.PROCESSING
                };
                sale.CalculateAmoutValue();
                await this.saleServices.Save(sale);
                await this.rabbitMQProducer.Publish(eventName: "add-sale", data: "Venda salva com sucesso!");
                var saleMessage = new SaleMessage
                {
                    Id = sale.Id,
                    ProductId = sale.ProductId,
                    Quantity = sale.Quantity,
                    Status = sale.Status.ToString()
                };
                await this.rabbitMQProducer.Publish(eventName: "validate-product", data: saleMessage);
                return CreatedAtAction(nameof(GetById), new { Id = sale.Id }, sale);
            }
            var erros = ModelState.Values.Select(x => x.Errors);
            return BadRequest(erros);
        }
        [HttpPut("{Id}")]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Atualiza o registro de venda",
            Description = "Busca e atualiza o registro de uma venda no banco de dados."
        )]
        [SwaggerResponse(200, "Venda atualizada com sucesso.")]
        [SwaggerResponse(400, "Erros de validação")]
        [SwaggerResponse(404, "Venda não encontrada.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> Update([FromRoute] int Id, SaleRequest saleRequest)
        {
            if (ModelState.IsValid)
            {
                var saleDb = await this.saleServices.GetById(Id);
                if (saleDb == null)
                    return NotFound(new { message = "Venda não encontrada." });
                if (saleDb.Status == SaleStatus.CONFIRMED)
                    return BadRequest(new { message = "Não é possivel atualizar um produto de um pedido finalizado." });
                saleDb.Id = Id;
                saleDb.ProductId = saleRequest.ProductId;
                saleDb.Quantity = saleRequest.Quantity;
                saleDb.UnitPrice = saleRequest.UnitPrice;
                saleDb.CalculateAmoutValue();
                await this.saleServices.Update(saleDb);
                await this.rabbitMQProducer.Publish(eventName: "update-sale", data: new { saleId = saleDb.Id });
                return Ok(new { message = "Venda atualizada com sucesso!" });
            }
            var erros = ModelState.Values.Select(x => x.Errors);
            return BadRequest(erros);
        }
        [HttpDelete("{Id}")]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Deleta um registro de venda",
            Description = "Deleta um registro de uma venda no banco de dados."
        )]
        [SwaggerResponse(200, "Venda deletada com sucesso.")]
        [SwaggerResponse(404, "Venda não encontrada.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> DeleteByIdInRoute([FromRoute] int Id)
        {
            var sale = await this.saleServices.GetById(Id);
            if (sale == null)
                return NotFound();
            await this.saleServices.Delete(sale);
            await this.rabbitMQProducer.Publish(eventName: "delete-sale", data: new { saleId = sale.Id });
            return Ok(new { message = "Registro deletado com sucesso!" });
        }
        [HttpDelete]
        [Authorize(Roles = "Administrador,Client")]
        [SwaggerOperation(
            Summary = "Deleta um registro de venda",
            Description = "Deleta um registro de uma venda no banco de dados."
        )]
        [SwaggerResponse(200, "Venda deletada com sucesso.")]
        [SwaggerResponse(404, "Venda não encontrada.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> DeleteById([FromQuery] int Id)
        {
            var sale = await this.saleServices.GetById(Id);
            if (sale == null)
                return NotFound(new { message = "Venda não encontrada." });
            await this.saleServices.Delete(sale);
            await this.rabbitMQProducer.Publish(eventName: "delete-sale", data: new { saleId = sale.Id });
            return Ok(new { message = "Registro deletado com sucesso!" });
        }
    }
}