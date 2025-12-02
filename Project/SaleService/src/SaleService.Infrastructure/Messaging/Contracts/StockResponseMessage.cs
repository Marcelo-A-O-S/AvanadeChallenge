namespace SaleService.Infrastructure.Messaging.Contracts
{
    public class StockResponseMessage
    {
        public long OrderId { get; set; }
        public long? SaleId { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}