using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaleService.Domain.Entities
{
    public class SalesInfoProduct
    {
        public long ProductId { get; set; }
        public int TotalSales { get; set; }
    }
}