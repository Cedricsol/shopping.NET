using System.ComponentModel.DataAnnotations;

namespace Shopping.NET.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Veuillez entrer un email")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        [MaxLength(255, ErrorMessage = "Email trop long")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Veuillez entrer un nom d'utilsateur")]
        [MinLength(3, ErrorMessage = "Le nom d'utilisateur doit contenir au minimum 3 caractères")]
        [MaxLength(50, ErrorMessage = "Le nom d'utilisateur est trop long")]
        [RegularExpression(@"^[a-zA-Z0-9_-+$", ErrorMessage = "Cararctères invalides")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Veuillez entrer un mot de passe")]
        [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères")]
        public string Password { get; set; } = string.Empty;

    }
}
