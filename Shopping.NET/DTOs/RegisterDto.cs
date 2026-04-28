using System.ComponentModel.DataAnnotations;

namespace Shopping.NET.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Veuillez entrer un email")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Veuillez entrer un nom d'utilsateur")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Veuillez entrer un mot de passe")]
        [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
        public string Password { get; set; } = string.Empty;

    }
}
