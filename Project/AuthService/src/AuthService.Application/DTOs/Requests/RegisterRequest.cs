using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.DTOs.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "O campo name é obrigatório")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O campo email é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo senha é obrigatório")]
        public string Password { get; set; }
        [Required(ErrorMessage = "O campo de confirmação de senha é obrigatório")]
        [Compare("Password",ErrorMessage ="Campo não confere!")]
        public string PasswordConfirm { get; set; }
    }
}