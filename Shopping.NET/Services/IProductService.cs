using Shopping.NET.Models;

namespace Shopping.NET.Services
{
    public interface IProductService
    {
        Product CreateProduct(Product product);
        List<Product> GetAllProducts();
    }
}
