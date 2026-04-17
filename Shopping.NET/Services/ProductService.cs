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

        public Product CreateProduct(Product product)
        {
            if (!_dbContext.Database.CanConnect())
            {
                _logger.LogError("Database connection failed");
                throw new Exception("DB connection failed");
            }

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            _logger.LogInformation("Product created with ID: {Id}", product.Id);

            return product;
        }

        public List<Product> GetAllProducts()
        {
            if (!_dbContext.Database.CanConnect())
            {
                _logger.LogError("Database connection failed");
                throw new Exception("DB connection failed");
            }

            _logger.LogInformation("Size of the list of products : " + _dbContext.Products.ToList().Count().ToString());

            return _dbContext.Products.ToList();
        }
    }
}
