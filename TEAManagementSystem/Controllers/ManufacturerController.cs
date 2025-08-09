using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TEAManagementSystem.Models;
using TEAManagementSystem.Services;
namespace TEAManagementSystem.Controllers

{


    [Route("api/manufacturer")]
    [ApiController]
    public class ManufacturerController : ControllerBase

    {
        private readonly ManufacturerService service = new ManufacturerService();
        private readonly IConfiguration _configuration;

        public ManufacturerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Ïnvallid login request");
            }

            var user = service.GetByLogin(model.Email, model.Password);
            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { Password = user.Password, Email = user.Email });

        }

        [HttpPost("addProduct")]
        public IActionResult Add([FromBody] ProductDto dto)
        {
            if (dto == null || dto.ManufacturerId == 0)
                return BadRequest("ManufacturerId is required.");

            var product = new Product
            {
                ProductType = dto.ProductType,
                CostPrice = dto.CostPrice,
                SellingPrice = dto.SellingPrice,
                FullQuantity = dto.FullQuantity,
                ManufacturerId = dto.ManufacturerId
                // DateAdded and others handled automatically
            };

            bool result = service.Add(product);
            return result ? Ok("Product added successfully") : BadRequest("Error adding product");
        }

        [HttpPost("deleteProduct")]
        public IActionResult Delete([FromBody] Product model){

            if(model ==null||string.IsNullOrEmpty(model.ProductType)){
            return BadRequest("Invalid product data");
            }
          


     bool result = service.Delete(model.ProductType);
         return result? Ok("Product deleted successfully") : BadRequest("Error deleting product");
    }





    }
}
