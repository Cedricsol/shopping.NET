using Microsoft.AspNetCore.Mvc;
using Moq;
using Shopping.NET.Controllers;
using Shopping.NET.DTOs;
using Shopping.NET.Services;

namespace Shopping.NET.Tests.Controllers
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task RegisterProduct_Should_Add_Product_And_Return_Ok()
        {
            // Setup
            var mockService = new Mock<IProductService>();

            var product = new CreateProductDto
            {
                Name = "Test",
                Price = 10,
                ImageUrl = "img.jpg",
            };

            var returnedDto = new ProductDto
            {
                Id = 1,
                Name = "Test",
                Price = 10,
                ImageUrl = "img.jpg",
            };

            mockService
                .Setup(s => s.CreateProduct(product))
                .ReturnsAsync(returnedDto);

            var controller = new ProductController(mockService.Object);


            var result = await controller.RegisterProduct(product);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<ProductDto>(okResult.Value);

            Assert.Equal("Test", returnedProduct.Name);
        }

        [Fact]
        public async Task GetProducts_Should_Return_All_Products()
        {
            // Setup
            var mockService = new Mock<IProductService>();

            var fakeProducts = new List<ProductDto>
            {
                new ProductDto {Name = "P1", Price = 10, ImageUrl = "p1.jpg"},
                new ProductDto {Name = "P2", Price = 42, ImageUrl = "p2.jpg"}
            };

            mockService
                .Setup(s => s.GetAllProducts())
                .ReturnsAsync(fakeProducts);

            var controller = new ProductController(mockService.Object);

            var result = await controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsType<List<ProductDto>>(okResult.Value);

            Assert.Equal(2, products.Count);
        }
    }
}
