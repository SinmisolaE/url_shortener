using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URLShort.API.DTO;
using URLShort.Core.Exceptions;

namespace URLShort.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {

        private readonly IUrlService _service;
        private readonly ILogger<UrlController> _logger;

        public UrlController(IUrlService service, ILogger<UrlController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Welcome()
        {
            _logger.LogInformation("In application");
            return Ok("Welcome to URL shortener");
        }


        [HttpPost("generate")]
        public async Task<ActionResult<string>> GenerateShortUrl(UrlDTO urlDTO)
        {
            try
            {
                _logger.LogInformation("Trying to generate short url");
                var url = await _service.AddUrlAsync(urlDTO);

                return Ok(url);
            }
            catch (UrlTooLongException e)
            {
                _logger.LogError($"Error: {e.Message}");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }


        [HttpGet("{shortCode}")]
        public async Task<ActionResult<string>> GetUrl(string shortCode)
        {
            try
            {
                _logger.LogInformation("Trying to get long url");
                var url = await _service.GetUrlByShortUrlAsync(shortCode);

                BackgroundJob.Enqueue<IClickService>(x => x.RecordClick(shortCode));

                _logger.LogInformation("Redirectiong to {url}", url);
                return Redirect(url);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
