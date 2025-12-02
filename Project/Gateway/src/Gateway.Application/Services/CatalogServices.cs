using System.Net.Http.Headers;
using System.Net.Http.Json;
using Gateway.Application.DTOs;
using Gateway.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Gateway.Application.Services
{
    public class CatalogServices : ICatalogServices
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public CatalogServices(HttpClient _httpClient, IConfiguration _configuration)
        {
            this.httpClient = _httpClient;
            this.configuration = _configuration;
        }
        public async Task<object> GetCatalogAsync(string token, int page)
        {
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ",""));
            var stockUrl = this.configuration.GetSection("Microservices:Stock").Value;
            var saleUrl = this.configuration.GetSection("Microservices:Sale").Value;
            var products = await this.httpClient.GetFromJsonAsync<List<ProductDTO>>($"{stockUrl}/api/product/GetProductsWithStock?page={page}");
            if (products == null || products.Count == 0)
                return new List<object>();
            var productsId = products.Select(p=> p.Id).ToList();
            var queryString = string.Join("&productsId=", productsId);
            var sales = await this.httpClient
                .GetFromJsonAsync<List<SaleInfoProductDTO>>
                ($"{saleUrl}/api/sales/GetSalesInfoProductsConfirmedByIds?productsId={queryString}");
            var catalog = products.Select(p => 
            new{
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                TotalSales = sales.FirstOrDefault(s => s.ProductId == p.Id)?.TotalSales ?? 0
            });
            return catalog;
        }
    }
}