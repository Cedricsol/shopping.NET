namespace Shopping.NET.Models

{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; } //use decimal cause more precise

        public string ImageUrl { get; set; } = string.Empty;
    }
}
