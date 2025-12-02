using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IUserRepository : IGenerics<User>
    {
        Task<User> GetUserByEmail(string email);
        Task<int> GetQuantity();
    }
}