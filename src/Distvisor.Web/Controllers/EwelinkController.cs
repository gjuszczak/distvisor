using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EwelinkController : ControllerBase
    {
        private readonly IEwelinkClient _client;
        public EwelinkController(IEwelinkClient client)
        {
            _client = client;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task Test()
        {
            var p = await _client.GetAccessToken();
            var d = await _client.GetDevices(p);
        }
    }
}
