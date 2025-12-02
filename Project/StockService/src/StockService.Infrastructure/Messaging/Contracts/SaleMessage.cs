namespace StockService.Infrastructure.Messaging.Contracts
{
    public class SaleMessage
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Status { get; set; }
    }
}