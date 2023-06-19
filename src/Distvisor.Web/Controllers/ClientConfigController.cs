using Distvisor.Web.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/client-config")]
    public class ClientConfigController : ControllerBase
    {
        private readonly ClientConfiguration _clientConfig;

        public ClientConfigController(IOptions<ClientConfiguration> clientConfig, IWebHostEnvironment env)
        {
            _clientConfig = clientConfig.Value;
            _clientConfig.BackendDetails ??= new BackendDetails();
            _clientConfig.BackendDetails.AppVersion ??= 
                Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                ?? "unknown";
            _clientConfig.BackendDetails.RuntimeVersion ??= RuntimeInformation.FrameworkDescription;
            _clientConfig.BackendDetails.Environment ??= env.EnvironmentName;
        }

        [HttpGet]
        public ClientConfiguration GetClientConfig()
        {
            return _clientConfig;
        }
    }
}
