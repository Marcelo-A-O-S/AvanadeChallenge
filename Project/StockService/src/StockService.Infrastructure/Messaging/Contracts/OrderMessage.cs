namespace StockService.Infrastructure.Messaging.Contracts
{
    public class OrderMessage
    {
        public long Id { get; set; }
        public string Status { get; set; }
        public List<SaleMessage> Sales { get; set; }
    }
}