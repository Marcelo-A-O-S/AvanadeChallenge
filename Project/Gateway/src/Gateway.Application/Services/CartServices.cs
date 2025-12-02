using Gateway.Application.DTOs;
using Gateway.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Gateway.Application.Services
{
    public class CartServices : ICartServices
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        public CartServices(HttpClient _httpClient, IConfiguration _configuration)
        {
            this.httpClient = _httpClient;
            this.configuration = _configuration;
        }
        public async Task<object> GetCartProducts(string token, int userId, int page = 1)
        {
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            var stockUrl = this.configuration.GetSection("Microservices:Stock").Value;
            var saleUrl = this.configuration.GetSection("Microservices:Sale").Value;
            var sales = await this.httpClient.GetFromJsonAsync<List<SaleDTO>>($"{saleUrl}/api/sales/GetSalesInProgress?userId={userId}&page={page}");
            if (sales == null || sales.Count == 0)
                return new List<object>();
            var productIds = sales.Select(s => s.ProductId).Distinct().ToList();
            var queryString = string.Join("&ids=", productIds);
            var products = await this.httpClient
                .GetFromJsonAsync<List<ProductDTO>>($"{stockUrl}/api/product/ByIds?ids={queryString}");
            var cart = sales.Select(sale =>
            {
                var product = products.First(p=> p.Id == sale.ProductId);
                return new
                {
                    sale.Id,
                    sale.UserId,
                    sale.ProductId,
                    product.Name,
                    product.Description,
                    product.Price,
                    sale.Quantity,
                    sale.Status
                };
            });
            return cart;
        }
    }
}