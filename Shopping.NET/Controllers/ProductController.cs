using Microsoft.AspNetCore.Mvc;
using Shopping.NET.Models;

namespace Shopping.NET.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public ProductController (AppDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        [Route("register")]
        public IActionResult RegisterProduct(Product product)
        {
            var canConnect = _dbContext.Database.CanConnect();
            if (!canConnect) {
                return StatusCode(500, "DB connection failed.");
            }
            _dbContext.Add(product);
            _dbContext.SaveChanges();
            return Ok(product);
        }
    }
}
