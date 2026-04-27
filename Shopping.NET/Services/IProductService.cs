using Shopping.NET.DTOs;

namespace Shopping.NET.Services
{
    public interface IProductService
    {
        Task<ProductDto> CreateProduct(CreateProductDto product);
        Task<List<ProductDto>> GetAllProducts();
    }
}
