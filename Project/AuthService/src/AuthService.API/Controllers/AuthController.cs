using AuthService.Application.DTOs.Requests;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserServices userServices;
        private readonly IJwtBearerServices jwtBearerServices;
        public AuthController(IUserServices _userServices, IJwtBearerServices _jwtBearerServices)
        {
            this.userServices = _userServices;
            this.jwtBearerServices = _jwtBearerServices;
        }
        [HttpPost("Login")]
        [SwaggerOperation(
            Summary = "Login usuário do sistema",
            Description = "Realiza o login e retorna um token de autenticação"
        )]
        [SwaggerResponse(200, "Token de acesso retornado com sucesso.")]
        [SwaggerResponse(400, "Erro de validação.")]
        [SwaggerResponse(401, "Acesso negado.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await this.userServices.FindBy(x => x.Email == loginRequest.Email);
                if (user == null)
                    return Unauthorized(new { message = "Email não autorizado"});
                if (!await user.verifyPassword(loginRequest.Password))
                    return Unauthorized(new { message = "Senha inválida"});
                var token = await this.jwtBearerServices.GenerateJwtToken(user);
                return Ok(new { message = token });
            }
            var erros = ModelState.Values.Select(x => x.Errors);
            return BadRequest(erros);
        }
        [HttpPost("Register")]
        [SwaggerOperation(
            Summary = "Realiza o registro do usuário sistema",
            Description = "Realiza o registro do usuário no sistema."
        )]
        [SwaggerResponse(200, "Registro realizado com sucesso.")]
        [SwaggerResponse(400, "Erro de validação.")]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await this.userServices.FindBy(x => x.Email == registerRequest.Email);
                if (user != null)
                    return BadRequest(new { message = "Email de cadastro já constado no sistema, prossiga para realização do login"});
                user = new User();
                user.Id = 0;
                user.Email = registerRequest.Email;
                user.Name = registerRequest.Name;
                user.Role = Role.Client;
                user.createPasswordHash(registerRequest.Password);
                await this.userServices.Save(user);
                return Ok(new { message = "Registro realizado com sucesso!" });
            }
            var erros = ModelState.Values.Select(x => x.Errors);
            return BadRequest(erros);
        }
    }
}