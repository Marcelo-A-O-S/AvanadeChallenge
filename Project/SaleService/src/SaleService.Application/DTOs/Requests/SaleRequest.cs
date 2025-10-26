using System.ComponentModel.DataAnnotations;
namespace SaleService.Application.DTOs.Requests
{
    public class SaleRequest
    {
        [Required(ErrorMessage = "O campo ProductId é obrigatório.")]
        public long ProductId { get; set; }
        [Required(ErrorMessage = "O campo Quantity é obrigatório.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "O campo UnitPrice é obrigatório.")]
        public decimal UnitPrice { get; set; }
        public bool isValid { get; set; }
    }
}