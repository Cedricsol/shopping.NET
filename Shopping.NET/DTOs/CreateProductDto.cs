namespace Shopping.NET.DTOs
{
    public class CreateProductDto
    {
        // All data gotten from front
        public string Name { get; set; } = String.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = String.Empty;
    }
}
