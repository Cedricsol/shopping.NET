using Microsoft.EntityFrameworkCore;
using Shopping.NET.DTOs;
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

        public async Task<ProductDto> CreateProduct(CreateProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                ImageUrl = productDto.ImageUrl,
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Product created with ID: {Id}", product.Id);

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
            };
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            var products = await _dbContext.Products.ToListAsync();

            _logger.LogInformation("Size of the list of products : " + products.Count());

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
            }).ToList();
        }
    }
}
