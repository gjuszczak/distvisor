using Distvisor.Web.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IUpdateService
    {
        Task<IEnumerable<string>> GetReleasesAsync();
        Task UpdateToVersionAsync(string version, string dbUpdateStrategy);
    }

    public class UpdateService : IUpdateService
    {
        private readonly EnvConfiguration _env;
        private readonly IGithubClient _github;
        private readonly INotificationService _notifications;

        public UpdateService(IOptions<EnvConfiguration> env, IGithubClient github, INotificationService notifications, ISecretsVault keyVault)
        {
            _env = env.Value;
            _notifications = notifications;
            _github = github;

            var secrets = keyVault.GetGithubSecrets();
            github.Configure(secrets.RepoOwner, secrets.RepoName, secrets.ApiKey);
        }

        public async Task<IEnumerable<string>> GetReleasesAsync()
        {
            try
            {
                return await _github.GetReleasesAsync();
            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync("Unable to get available releases", exc);
                throw;
            }
        }

        public async Task UpdateToVersionAsync(string version, string dbUpdateStrategy)
        {
            try
            {
                await _github.UpdateToVersionAsync(_env.CurrentVersion, version, dbUpdateStrategy);

                await _notifications.PushSuccessAsync($"Update to {version} started...");
            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync($"Unable to start {version} update", exc);
                throw;
            }
        }        
    }
}
