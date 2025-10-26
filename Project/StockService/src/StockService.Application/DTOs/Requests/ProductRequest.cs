using System.ComponentModel.DataAnnotations;
namespace StockService.Application.DTOs.Requests
{
    public class ProductRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 e 100 caracteres.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Informar uma breve descrição do produto")]
        public string Description { get; set; }
        [Required(ErrorMessage = "O preço do produto é obrigatório")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "A quantidade do produto é obrigatória")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "A quantidade de estoque minimo do produto é obrigatória")]
        public int MinimunStock { get; set; }
    }
}