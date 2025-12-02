namespace SaleService.Application.DTOs.Responses
{
    public class OrderResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Status { get; set;}
        public DateTime CreatedAt { get; set; }
        public List<SaleResponse> Sales { get; set; }
    }
}