using Microsoft.AspNetCore.Mvc;
using TEAManagementSystem.Services;
using TEAManagementSystem.Models;
namespace TEAManagementSystem.Controllers
{
    [Route("api/request")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly RequestService service = new RequestService();

        [HttpPost("create")]
        public IActionResult CreateRequest([FromBody] Request request)
        {
            bool result = service.CreateRequest(request);
            return result ? Ok("Request created successfully") : BadRequest("Cannot create request");
        }
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var list = service.GetAll();
            return Ok(list);
        }

        [HttpGet("get/{RequestId}")]
        public IActionResult GetById(int RequestId)
        {
            var request = service.GetById(RequestId);
            return request == null ? NotFound("Request not found") : Ok(request);
        }



        [HttpPost("approve/{RequestId}")]
        public IActionResult Approve([FromRoute] int RequestId)
        {
            bool result = service.ApproveRequest(RequestId);
            return result ? Ok("Request approved successfully") : BadRequest("Cannot approve request");
        }

        [HttpPost("reject/{RequestId}")]
        public IActionResult Reject([FromRoute] int RequestId)
        {
            bool result = service.RejectRequest(RequestId);
            return result ? Ok("Request rejected successfully") : BadRequest("Cannot reject request");
        }

        [HttpPost("UpdateRequestStatus")]
        public IActionResult UpdateRequestStatus(int RequestId, string Status)
        {
            bool success = service.UpdateRequestStatus(RequestId, Status);
            return success ? Ok("Request status updated successfully") : BadRequest("Cannot update request status");
        }


    }
}

