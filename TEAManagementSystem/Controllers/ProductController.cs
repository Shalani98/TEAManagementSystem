using Microsoft.AspNetCore.Mvc;
using TEAManagementSystem.Services;
namespace TEAManagementSystem.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController:ControllerBase
    {
        private readonly ProductService service=new ProductService();
        private readonly IConfiguration _configuration;

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var list = service.GetAll();
            return Ok(list);
        }
    }
}
