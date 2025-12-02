using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces
{
    public interface IJwtBearerServices
    {
        Task<string> GenerateJwtToken(User user);
    }
}