namespace SaleService.Application.DTOs.Responses
{
    public class SaleResponse
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}