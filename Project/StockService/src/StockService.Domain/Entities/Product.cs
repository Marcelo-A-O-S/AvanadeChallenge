using System.ComponentModel.DataAnnotations.Schema;
using StockService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace StockService.Domain.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int MinimunStock { get; set; }
        public void UpdateQuantity(int quantity, TypeMovement typeMovement)
        {
            if (quantity <= 0)
                throw new Exception("Quantidade informada inválida.");
            switch (typeMovement)
                {
                    case TypeMovement.Input:
                        Quantity += quantity;
                        break;
                    case TypeMovement.Output:
                        if (Quantity < quantity)
                            throw new Exception("Estoque insuficiente para realizar a saída.");
                        Quantity -= quantity;
                        break;
                    default:
                        throw new Exception("Tipo de movimentação inválido.");
                }
        }
    }
}