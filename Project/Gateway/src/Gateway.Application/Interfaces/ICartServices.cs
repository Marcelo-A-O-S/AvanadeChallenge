namespace Gateway.Application.Interfaces
{
    public interface ICartServices
    {
        Task<object> GetCartProducts(string token, int userId, int page = 1);
    }
}