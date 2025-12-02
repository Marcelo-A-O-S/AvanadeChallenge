using StockService.Domain.Entities;
namespace StockService.Infrastructure.Messaging.Contracts
{
    public class ProductMessage
    {
        public Product Product { get; set; }
        public string MessageResponse { get; set; }
    }
}