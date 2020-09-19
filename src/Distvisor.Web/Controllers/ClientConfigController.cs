using Distvisor.Web.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientConfigController : ControllerBase
    {
        private readonly ClientConfiguration _clientConfig;

        public ClientConfigController(IOptions<ClientConfiguration> clientConfig, IWebHostEnvironment env)
        {
            _clientConfig = clientConfig.Value;

            if(_clientConfig.BackendDetails == null)
            {
                _clientConfig.BackendDetails = new BackendDetails();
            }

            if (_clientConfig.BackendDetails.AppVersion == null)
            {
                _clientConfig.BackendDetails.AppVersion =
                    Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            }

            if (_clientConfig.BackendDetails.RuntimeVersion == null)
            {
                _clientConfig.BackendDetails.RuntimeVersion = RuntimeInformation.FrameworkDescription;
            }

            if (_clientConfig.BackendDetails.Environment == null)
            {
                _clientConfig.BackendDetails.Environment = env.EnvironmentName;
            }
        }

        [HttpGet]
        public ClientConfiguration GetClientConfig()
        {
            return _clientConfig;
        }
    }
}
