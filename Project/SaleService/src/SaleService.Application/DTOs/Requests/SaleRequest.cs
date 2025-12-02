using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SaleService.Domain.Enums;
namespace SaleService.Application.DTOs.Requests
{
    public class SaleRequest
    {
        [Required(ErrorMessage = "O campo UserId é obrigatório.")]
        [Range(1, long.MaxValue, ErrorMessage = "O Id do usuário deve ser maior que zero.")]
        public long UserId { get; set; }
        [Required(ErrorMessage = "O campo ProductId é obrigatório.")]
        [Range(1, long.MaxValue, ErrorMessage = "O Id do produto deve ser maior que zero.")]
        public long ProductId { get; set; }
        [Required(ErrorMessage = "O campo Quantity é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O quantidade do produto deve ser maior que zero.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "O campo UnitPrice é obrigatório.")]
        public decimal UnitPrice { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SaleStatus Status { get; set; }
    }
}