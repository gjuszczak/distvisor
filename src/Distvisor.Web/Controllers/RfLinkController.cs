using Distvisor.Web.Configuration;
using Distvisor.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/rf-link")]
    public class RfLinkController : ControllerBase
    {
        private readonly RfLinkConfiguration _config;
        private readonly ICryptoService _cryptoService;
        private readonly IHomeBoxService _homeBoxService;

        public RfLinkController(IOptions<RfLinkConfiguration> config, ICryptoService cryptoService, IHomeBoxService homeBoxService)
        {
            _config = config.Value;
            _cryptoService = cryptoService;
            _homeBoxService = homeBoxService;
        }

        [HttpPost]
        public async Task<IActionResult> RfCodeReceived([Required, FromQuery] string code, [Required, FromQuery] long timestamp, [Required, FromHeader] string authorization)
        {
            if (authorization.StartsWith("sign ", StringComparison.InvariantCultureIgnoreCase))
            {
                authorization = authorization.Substring("sign ".Length);
            }

            var expectedSignature = _cryptoService.HmacSha256Base64(HttpContext.Request.QueryString.Value, _config.HmacKey);

            if (authorization != expectedSignature)
            {
                return Unauthorized();
            }

            await _homeBoxService.RfCodeReceivedAsync(code);

            return Ok();
        }
    }
}
