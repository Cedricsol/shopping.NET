using Shopping.NET.Models;

namespace Shopping.NET.Services
{
    public interface IProductService
    {
        Task<Product> CreateProduct(Product product);
        Task<List<Product>> GetAllProducts();
    }
}
