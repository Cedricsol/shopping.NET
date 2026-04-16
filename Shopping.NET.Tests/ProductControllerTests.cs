using Microsoft.AspNetCore.Mvc;
using Shopping.NET.Controllers;
using Shopping.NET.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.NET.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void RegisterProduct_Should_Add_Product_And_Return_Ok()
        {
            // Setup
            var context = TestDbContextFactory.Create();
            var controller = new ProductController(context);

            var product = new Product
            {
                Name = "Test",
                Price = 10,
                ImageUrl = "img.jpg",
            };


            var result = controller.RegisterProduct(product);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);

            Assert.Equal("Test", returnedProduct.Name);
            Assert.Equal(1, context.Products.Count());
        }

        [Fact]
        public void GetProducts_Should_Return_All_Products()
        {
            // Setup
            var context = TestDbContextFactory.Create();

            context.Products.Add(new Product { Name = "P1", Price = 10 });
            context.Products.Add(new Product { Name = "P2", Price = 42 });
            context.SaveChanges();

            var controller = new ProductController(context);

            var result = controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsType<List<Product>>(okResult.Value);

            Assert.Equal(2, products.Count);
        }
    }
}
