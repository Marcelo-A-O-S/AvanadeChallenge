using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SaleService.Domain.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; private set; }
        public List<Sale> Sales { get; set; }
        public bool IsCanceled { get; set; }
        public void Cancel()
        {
            if (this.IsCanceled)
                throw new Exception("O pedido já foi cancelado.");

            this.IsCanceled = true;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}