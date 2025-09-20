using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// controller to ensure middleware is working
namespace URLShort.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpGet("/throw")]
        public IActionResult throwCode()
        {
            throw new Exception("Testing my middleware");
        }
    }
}
