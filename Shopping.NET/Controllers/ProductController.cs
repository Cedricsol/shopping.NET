using Microsoft.AspNetCore.Mvc;
using Shopping.NET.DTOs;
using Shopping.NET.Services;

namespace Shopping.NET.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController (IProductService service )
        {
           _productService  = service;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterProduct([FromBody] CreateProductDto product)
        {
            var createdProduct = await _productService.CreateProduct(product);
            return Ok(createdProduct);
            
        }

        [HttpGet]
        public async  Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }
    }
}
