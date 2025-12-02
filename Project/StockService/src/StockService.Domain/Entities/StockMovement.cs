using StockService.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace StockService.Domain.Entities
{
    public class StockMovement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? OrderId { get; set; }
        public long? SaleId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public TypeMovement Type { get; set; }
        public ReasonMovement Reason { get; set; }
        public void Validate()
        {
            if (this.Quantity <= 0)
                throw new Exception(message: "Quantidade informada inválida");
            if (Type == TypeMovement.Input && Reason == ReasonMovement.Loss)
                throw new Exception("Entrada não pode ter motivo de perda.");
            if (Type == TypeMovement.Output && Reason == ReasonMovement.Purchase)
                throw new Exception("Saída não pode ter motivo de compra.");
            if (Reason == ReasonMovement.Sale && OrderId == null)
                throw new Exception("Movimentações de venda precisam de OrderId.");
        }
    }
}