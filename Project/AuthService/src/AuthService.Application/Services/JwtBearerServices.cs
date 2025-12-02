using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Application.Services
{
    public class JwtBearerServices : IJwtBearerServices
    {
        private readonly IConfiguration configuration;
        public JwtBearerServices(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }
        public async Task<string> GenerateJwtToken(User user)
        {
            var key = this.configuration.GetSection("JWT:Key").Value ?? "sdnsieisnicpanwdcse1518106";
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("email", user.Email));
            claims.Add(new Claim("name", user.Name));
            claims.Add(new Claim("role", user.Role.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));
            var sercurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(sercurityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddHours(8)
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}