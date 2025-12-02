using SaleService.Domain.Entities;
using SaleService.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SaleService.Application.Validators;
namespace SaleService.Application.DTOs.Requests
{
    public class OrderRequest
    {
        [Required(ErrorMessage = "O campo UserId é obrigatório.")]
        public long UserId { get; set; }
        [MinCollectionCount(1, ErrorMessage = "O pedido deve conter pelo menos uma venda.")]
        public List<Sale> Sales { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus Status { get; set; }
    }
}