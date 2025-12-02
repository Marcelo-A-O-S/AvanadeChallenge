using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "O campo email é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo password é obrigatório")]
        public string Password { get; set; }
    }
}