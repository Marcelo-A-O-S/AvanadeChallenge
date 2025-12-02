using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateway.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Gateway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartServices cartServices;
        public CartController(ICartServices _cartServices)
        {
            this.cartServices = _cartServices;
        }
        [HttpGet]
        [Authorize(Roles = "Administrador,Client")]
        public async Task<IActionResult> GetProducts([FromQuery] int userId,[FromQuery] int page = 1)
        {
            var token = HttpContext.Request.Headers.Authorization
                .ToString();
            var catalog = await this.cartServices.GetCartProducts(token,userId);
            return Ok(catalog);
        }
    }
}