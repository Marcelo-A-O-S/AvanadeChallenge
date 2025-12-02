using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Application.DTOs
{
    public class SaleDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Status { get;set; }
    }
}