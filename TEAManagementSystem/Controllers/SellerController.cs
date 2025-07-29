using Microsoft.AspNetCore.Mvc;
using TEAManagementSystem.Models;
using TEAManagementSystem.Services;
namespace TEAManagementSystem.Controllers
{
    [Route("api/seller")]
    [ApiController]
    public class SellerController : ControllerBase

    {
        private readonly SellerService service = new SellerService();
        private readonly IConfiguration _configuration;

        public SellerController(IConfiguration configuration)
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

        [HttpGet("products/{sellerId}")]
        public IActionResult GetSellerProducts(int sellerId)
        {
            var products = service.GetSellerProducts(sellerId);
            return Ok(products);
        }

        [HttpPost("products/add")]
        public IActionResult AddSellerProduct([FromBody] SellerProductCreateDto model)
        {
            if (model == null || model.SellerId <= 0 || model.ProductId <= 0 || model.SellingPrice <= 0 || model.Quantity <= 0)
            {
                return BadRequest("Invalid input.");
            }

            var sellerProduct = new SellerProduct
            {
                SellerId = model.SellerId,
                ProductId = model.ProductId,
                Quantity = model.Quantity,
                SellingPrice = model.SellingPrice
                // CostPrice will be filled in service
            };

            bool success = service.AddSellerProduct(sellerProduct);
            if (success)
                return Ok(new { message = "Product added to seller stock." });

            return BadRequest("Failed to add seller product.");
        }


/// /manually adding mapper for dtos

        [HttpPost("test/add")]
        public IActionResult TestAdd()
        {
            var dto = new SellerProductCreateDto
            {
                SellerId = 1,
                ProductId = 2,
                Quantity = 50,
                SellingPrice = 300
            };

            var entity = new SellerProduct
            {
                SellerId = dto.SellerId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                SellingPrice = dto.SellingPrice,
                QuantitySold = 0,
                DateAdded = DateTime.Now
            };

            bool success = service.AddSellerProduct(entity);
            if (success)
                return Ok(new { message = "Test product added to seller stock." });

            return BadRequest("Failed to add test seller product.");
        }
    }
    }