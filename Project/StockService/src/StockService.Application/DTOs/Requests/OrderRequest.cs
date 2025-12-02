using System;
using System.ComponentModel.DataAnnotations;

namespace StockService.Application.DTOs.Requests
{
    public class OrderRequest
    {
        [Required(ErrorMessage = "O Id do pedido é obrigatório")]
        public long Id { get; set; }
        [Required(ErrorMessage ="O status do pedido é obrigatório")]
        public string Status { get; set; }
        public List<SaleRequest> Sales { get; set; }
    }
}


