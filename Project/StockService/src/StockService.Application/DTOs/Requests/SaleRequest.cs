namespace StockService.Application.DTOs.Requests
{
    public class SaleRequest
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Status { get; set; }
    }
}