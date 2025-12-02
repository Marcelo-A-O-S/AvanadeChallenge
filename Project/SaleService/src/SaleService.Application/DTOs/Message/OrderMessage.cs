using System.Collections.Generic;
namespace SaleService.Application.DTOs.Message
{
    public class OrderMessage
    {
        public long Id { get; set; }
        public bool IsCanceled { get; set; }
        public string Status { get; set; }
        public List<SaleMessage> Sales { get; set; }
    }
}