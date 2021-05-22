using Distvisor.Web.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IEwelinkClientWebSocketFactory
    {
        Task Open(string accessToken, string apiKey);
    }

    public class EwelinkClientWebSocketFactory : IEwelinkClientWebSocketFactory
    {
        private readonly IOptions<EwelinkConfiguration> _config;

        private IDisposable _ews;

        public EwelinkClientWebSocketFactory(IOptions<EwelinkConfiguration> config)
        {
            _config = config;
        }

        public async Task Open(string accessToken, string apiKey)
        {
            var ews = new EwelinkClientWebSocket(_config);
            await ews.Open(accessToken, apiKey);
            _ews = ews;
        }
    }
}
