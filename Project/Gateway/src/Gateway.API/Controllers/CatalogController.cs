using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateway.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogServices catalogServices;
        public CatalogController(ICatalogServices _catalogServices)
        {
            this.catalogServices = _catalogServices;
        }
        [HttpGet]
        [Authorize(Roles = "Administrador,Client")]
        public async Task<IActionResult> GetProducts([FromQuery] int page)
        {
            var token = HttpContext.Request.Headers.Authorization
                .ToString();
            var catalog = await this.catalogServices.GetCatalogAsync(token,page);
            return Ok(catalog);
        }
    }
}