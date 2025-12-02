namespace Gateway.Application.Interfaces
{
    public interface ICatalogServices
    {
        Task<object> GetCatalogAsync(string token, int page);
    }
}