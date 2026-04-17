using Shopping.NET.Models;

namespace Shopping.NET.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;

        public ProductService(AppDbContext context)
        {
            _dbContext = context;
        }

        public Product CreateProduct(Product product)
        {
            if (!_dbContext.Database.CanConnect())
            {
                throw new Exception("DB connection failed");
            }

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
            return product;
        }

        public List<Product> GetAllProducts()
        {
            if (!_dbContext.Database.CanConnect())
            {
                throw new Exception("DB connection failed");
            }

            return _dbContext.Products.ToList();
        }
    }
}
