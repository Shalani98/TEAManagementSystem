using Microsoft.AspNetCore.Mvc;
using TEAManagementSystem.Models;
using TEAManagementSystem.Services;

namespace TEAManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService = new OrderService();

        // POST: api/Order
        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderDTO dto)
        {
            bool success = _orderService.CreateOrder(dto);
            if (success)
                return Ok(new { message = "Order created successfully" });

            return BadRequest(new { message = "Failed to create order" });
        }

        // PUT: api/Order/approve
        [HttpPut("approve")]
        public IActionResult ApproveOrder([FromBody] UpdateOrderApprovalDTO dto, [FromQuery] int sellerId)
        {
            bool success = _orderService.ApproveOrder(dto, sellerId);
            if (success)
                return Ok(new { message = "Order approval status updated" });

            return BadRequest(new { message = "Failed to update approval status" });
        }

        // PUT: api/Order/payment
        [HttpPut("payment")]
        public IActionResult UpdatePaymentStatus([FromBody] UpdatePaymentStatusDTO dto)
        {
            bool success = _orderService.UpdatePaymentStatus(dto);
            if (success)
                return Ok(new { message = "Payment status updated" });

            return BadRequest(new { message = "Failed to update payment status" });
        }

        // GET: api/Order/{id}
        [HttpGet("{OrderId}")]
        public IActionResult GetOrderById(int OrderId)
        {
            var order = _orderService.GetOrderById(OrderId);
            if (order == null)
                return NotFound(new { message = "Order not found" });

            return Ok(order);
        }
        [HttpGet("all")]
        public IActionResult GetAllOrders()
        {
            var order = _orderService.GetAllOrders();
            return Ok(order);
        }
    }
}