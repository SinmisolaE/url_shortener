using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URLShort.API.DTO;
using URLShort.API.Interfaces;

namespace URLShort.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {

        private readonly IUrlService _service;

        public UrlController(IUrlService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<string> Welcome()
        {
            return Ok("Welcome to URL shortener");
        }


        [HttpPost("/generate")]
        public async Task<ActionResult<string>> GenerateShortUrl(UrlDTO urlDTO)
        {
            try
            {
                var url = await _service.AddUrlAsync(urlDTO);

                return Ok(url);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("{shortCode}")]
        public async Task<ActionResult<string>> GetUrl(string shortCode)
        {
            try
            {
                var url = await _service.GetUrlByShortUrlAsync(shortCode);

                return Redirect(url);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
