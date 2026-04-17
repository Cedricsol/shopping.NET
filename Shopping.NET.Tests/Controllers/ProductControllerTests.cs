using Microsoft.AspNetCore.Mvc;
using Moq;
using Shopping.NET.Controllers;
using Shopping.NET.Models;
using Shopping.NET.Services;

namespace Shopping.NET.Tests.Controllers
{
    public class ProductControllerTests
    {
        [Fact]
        public void RegisterProduct_Should_Add_Product_And_Return_Ok()
        {
            // Setup
            var mockService = new Mock<IProductService>();

            var product = new Product
            {
                Name = "Test",
                Price = 10,
                ImageUrl = "img.jpg",
            };

            mockService
                .Setup(s => s.CreateProduct(product))
                .Returns(product);

            var controller = new ProductController(mockService.Object);


            var result = controller.RegisterProduct(product);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);

            Assert.Equal("Test", returnedProduct.Name);
        }

        [Fact]
        public void GetProducts_Should_Return_All_Products()
        {
            // Setup
            var mockService = new Mock<IProductService>();

            var fakeProducts = new List<Product>
            {
                new Product {Name = "P1", Price = 10, ImageUrl = "p1.jpg"},
                new Product {Name = "P2", Price = 42, ImageUrl = "p2.jpg"}
            };

            mockService
                .Setup(s => s.GetAllProducts())
                .Returns(fakeProducts);

            var controller = new ProductController(mockService.Object);

            var result = controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsType<List<Product>>(okResult.Value);

            Assert.Equal(2, products.Count);
        }
    }
}
