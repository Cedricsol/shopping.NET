using System.ComponentModel.DataAnnotations;

namespace Shopping.NET.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Veuillez entrez votre email")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Veuillez entrer votre mot de passe")]
        public string Password { get; set; } = string.Empty;
    }
}
