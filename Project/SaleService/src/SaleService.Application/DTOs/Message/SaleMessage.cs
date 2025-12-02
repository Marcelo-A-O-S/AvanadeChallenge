using SaleService.Domain.Enums;
namespace SaleService.Application.DTOs.Message
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