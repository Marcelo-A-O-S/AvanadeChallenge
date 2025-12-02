using Microsoft.AspNetCore.Mvc;

namespace SaleService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(new { message = "Health"});
        }
    }
}