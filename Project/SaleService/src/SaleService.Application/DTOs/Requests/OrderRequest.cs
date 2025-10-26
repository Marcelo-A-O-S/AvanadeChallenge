using SaleService.Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace SaleService.Application.DTOs.Requests
{
    public class OrderRequest
    {
        [Required(ErrorMessage = "O campo UserId é obrigatório.")]
        public long UserId { get; set; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; private set; }
        public List<Sale> Sales { get; set; }
        public bool IsCanceled { get; set; }
    }
}