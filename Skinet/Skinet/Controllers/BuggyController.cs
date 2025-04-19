using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Skinet.DTOs;

namespace Skinet.Controllers
{
    public class BuggyController : BaseApiController
    {
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized();
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("not a good request");
        }

        [HttpGet("not found")]
        public IActionResult NotFound()
        {
            return NotFound();
        }

        [HttpGet("internalerror")]
        public IActionResult InternalError()
        {
            //throw an exception that it will be customise the  default response by the exceptionMiddleware class
            throw new Exception("This is a test exception");
        }

        [HttpPost("validationerror")]
        public IActionResult GetValidationError(CreateProductDto product)
        {
            return Ok();
        }
    }
}
