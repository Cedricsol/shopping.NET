

using Shopping.NET.Models;
using Shopping.NET.Services;

namespace Shopping.NET.Tests.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public void CreateProduct_Should_Add_Product_To_Db()
        {
            // Setup
            var context = TestDbContextFactory.Create();
            var service = new ProductService(context);

            var product = new Product
            {
                Name = "Test",
                Price = 10,
                ImageUrl = "test.jpg",
            };

            var result = service.CreateProduct(product);

            // Assert
            Assert.Equal(1, context.Products.Count());
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public void GetAllProducts_Should_Return_All_Products()
        {
            // Setup
            var context = TestDbContextFactory.Create();

            context.Products.Add(new Product { Name = "P1", Price = 10, ImageUrl = "p1.jpg"});
            context.Products.Add(new Product { Name = "P2", Price = 42, ImageUrl = "p2.jpg" });
            context.SaveChanges();

            var service = new ProductService(context);

            var result = service.GetAllProducts();

            Assert.Equal(2, result.Count);
            Assert.Equal("P1", result[0].Name);
        }
    }
}
