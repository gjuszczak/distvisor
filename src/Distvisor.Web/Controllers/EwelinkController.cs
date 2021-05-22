using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EwelinkController : ControllerBase
    {
        private readonly IEwelinkClient _client;
        private readonly IEwelinkClientWebSocketFactory _ewsf;

        public EwelinkController(IEwelinkClient client, IEwelinkClientWebSocketFactory ewsf)
        {
            _client = client;
            _ewsf = ewsf;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task Test()
        {
            var p = await _client.GetAccessToken();
            var d = await _client.GetDevices(p);
            //await _ewsf.Open(p, d.devicelist.First().apikey);
            await _client.SetDeviceStatus(p, d.devicelist.Last().deviceid, new { @switch = "toggle", bright = 100, colorB = 255, colorG = 255, colorR = 255 });
        }
    }
}
