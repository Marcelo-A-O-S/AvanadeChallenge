using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using StockService.Domain.Enums;
namespace StockService.Application.DTOs.Requests
{
    public class StockMovementRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = "O Id do pedido deve ser maior que zero.")]
        public long? OrderId { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "O Id do pedido deve ser maior que zero.")]
        public long? SaleId { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = "O Id do produto deve ser maior que zero.")]
        [Required(ErrorMessage = "O Id do produto é obrigatório")]
        public long ProductId { get; set; }
        [Required(ErrorMessage = "A quantidade do produto é obrigatória")]
        [Range(1, int.MaxValue, ErrorMessage = "O quantidade do produto deve ser maior que zero.")]
        public int Quantity { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TypeMovement Type { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ReasonMovement Reason { get; set; }
    }
}