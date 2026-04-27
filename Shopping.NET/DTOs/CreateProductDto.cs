using System.ComponentModel.DataAnnotations;

namespace Shopping.NET.DTOs
{
    public class CreateProductDto
    {
        // All data gotten from front
        [Required(ErrorMessage = "Veuillez entrer un nom de produit")]
        public string Name { get; set; } = String.Empty;
        [Range(0.00, double.MaxValue, ErrorMessage = "Le prix ne peut pas être négatif")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Veuillez entrer un chemin d'accès pour l'image sous le format '/images/file-name.extension'")]
        public string ImageUrl { get; set; } = String.Empty;
    }
}
