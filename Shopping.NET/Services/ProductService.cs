using Microsoft.EntityFrameworkCore;
using Shopping.NET.Models;

namespace Shopping.NET.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(AppDbContext context, ILogger<ProductService> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Product created with ID: {Id}", product.Id);

            return product;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var products = await _dbContext.Products.ToListAsync();
            _logger.LogInformation("Size of the list of products : " + products.Count());

            return products;
        }
    }
}
