namespace StockService.Infrastructure.Messaging.Contracts
{
    public class ProductResponseMessage
    {
        public long SaleId { get; set; }
        public long ProductId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}