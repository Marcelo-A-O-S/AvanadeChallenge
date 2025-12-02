using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SaleService.Domain.Enums;

namespace SaleService.Domain.Entities
{
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public SaleStatus Status { get; set; }
        public long? OrderId {get; set;}
        [ForeignKey(nameof(OrderId))]
        public Order Order {get; set;}
        public void CalculateAmoutValue()
        {
            this.TotalAmount = this.UnitPrice * this.Quantity;
        }
    }
}