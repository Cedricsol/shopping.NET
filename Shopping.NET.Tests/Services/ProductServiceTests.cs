using Microsoft.Extensions.Logging;
using Moq;
using Shopping.NET.DTOs;
using Shopping.NET.Models;
using Shopping.NET.Services;

namespace Shopping.NET.Tests.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task CreateProduct_Should_Add_Product_To_Db()
        {
            // Setup
            var context = TestDbContextFactory.Create();
            var mockLogger = new Mock<ILogger<ProductService>>();
            var service = new ProductService(context, mockLogger.Object);

            var product = new CreateProductDto
            {
                Name = "Test",
                Price = 10,
                ImageUrl = "test.jpg",
            };

            var result = await service.CreateProduct(product);

            // Assert
            Assert.Equal(1, context.Products.Count());
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public async Task GetAllProducts_Should_Return_All_Products()
        {
            // Setup
            var context = TestDbContextFactory.Create();
            var mockLogger = new Mock<ILogger<ProductService>>();

            context.Products.Add(new Product { Name = "P1", Price = 10, ImageUrl = "p1.jpg"});
            context.Products.Add(new Product { Name = "P2", Price = 42, ImageUrl = "p2.jpg" });
            await context.SaveChangesAsync();

            var service = new ProductService(context, mockLogger.Object);

            var result = await service.GetAllProducts();

            Assert.Equal(2, result.Count);
            Assert.Equal("P1", result[0].Name);
        }
    }
}
