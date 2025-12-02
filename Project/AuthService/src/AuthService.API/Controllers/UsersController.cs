using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices userServices;
        public UsersController(IUserServices _userServices)
        {
            this.userServices = _userServices;
        }
        [HttpGet("GetUserByEmail")]
        [Authorize(Roles = "Administrador,Client")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            var user = await this.userServices.GetUserByEmail(email);
            return Ok(user);
        }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> List([FromQuery] int? page)
        {
            var users = new List<User>();
            if (page.HasValue)
            {
                users = await this.userServices.List(page ?? 1);
            }
            else
            {
                users = await this.userServices.List();
            }
            return Ok(users);
        }
        [HttpGet("GetQuantity")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetQuantity()
        {
            var usersCount = await this.userServices.GetQuantity();
            return Ok(usersCount);
        }
    }
}