using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Distvisor.Web.Services
{
    public class RestClientProvider
    {
        private readonly bool _isRemoteRequestDisabled;
        private readonly INotificationService _notificationService;

        public RestClientProvider(IWebHostEnvironment env, IConfiguration config, INotificationService notificationService)
        {
            var isDevelop = env.IsDevelopment();
            var isRemoteRequestDisabledOnDev = config.GetValue<bool>("DisableRemoteRequestsOnDevEnv");
            _isRemoteRequestDisabled = isDevelop && isRemoteRequestDisabledOnDev;

            _notificationService = notificationService;
        }
    }
}
