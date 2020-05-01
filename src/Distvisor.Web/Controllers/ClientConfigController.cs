using Distvisor.Web.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientConfigController : ControllerBase
    {
        private readonly ClientConfiguration _clientConfig;

        public ClientConfigController(IOptions<ClientConfiguration> clientConfig)
        {
            _clientConfig = clientConfig.Value;
        }

        [HttpGet]
        public async Task<object> GetClientConfig()
        {
            return _clientConfig;
        }
    }
}
