using Microsoft.AspNetCore.Mvc;
using Shopping.NET.Models;

namespace Shopping.NET.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("test")]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult TestDb()
        {
            var canConnect = _context.Database.CanConnect();
            _context.Products.Add(new Product { Name = "Test" });
            _context.SaveChanges();
            return Ok(new {connected = canConnect});
        }
    }
}
