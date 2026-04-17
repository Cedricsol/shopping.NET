using Microsoft.AspNetCore.Mvc;
using Shopping.NET.Models;
using Shopping.NET.Services;

namespace Shopping.NET.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController (IProductService service )
        {
           _productService  = service;
        }

        [HttpPost]
        [Route("products")]
        public IActionResult RegisterProduct([FromBody] Product product)
        {
            try
            {
                var createdProduct = _productService.CreateProduct(product);
                return Ok(createdProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("products")]
        public IActionResult GetProducts()
        {
            try
            {
                var products = _productService.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
