using System.ComponentModel.DataAnnotations;

namespace Shopping.NET.DTOs
{
    public class CreateProductDto
    {
        // All data gotten from front
        [Required(ErrorMessage = "Name is mandatory")]
        public string Name { get; set; } = String.Empty;
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be higher than 0 or equal to 0")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Image is mandatory")]
        public string ImageUrl { get; set; } = String.Empty;
    }
}
