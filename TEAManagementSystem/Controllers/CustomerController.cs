using Microsoft.AspNetCore.Mvc;
using TEAManagementSystem.Models;
using TEAManagementSystem.Services;
namespace TEAManagementSystem.Controllers
{


    [Route("api/user")]
    [ApiController]
    public class CustomerController : ControllerBase

    {
        private readonly CustomerService service = new CustomerService();
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
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

        
        [HttpPost("register")]
        public IActionResult Register([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Invalid customer data");
            }

            bool result = service.Add(customer);

            if (result)
                return Ok("User registered successfully");
            else
                return BadRequest("Error registering user");
        }
    }
}
