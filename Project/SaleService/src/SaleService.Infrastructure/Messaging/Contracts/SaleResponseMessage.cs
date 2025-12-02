namespace SaleService.Infrastructure.Messaging.Contracts
{
    public class SaleResponseMessage
    {
        public long SaleId { get; set; }
        public long OrderId { get; set; }
        public string Message { get; set; }
        public string Status {get;set;}
    }
}