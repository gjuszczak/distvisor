using Distvisor.Web.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IDeploymentService
    {
        public string[] Environments { get; }

        Task<IEnumerable<string>> GetReleasesAsync();
        Task DeployVersionAsync(string environment, string version);
        Task RedeployAsync(string environment);
    }

    public class DeploymentService : IDeploymentService
    {
        private readonly DeploymentConfiguration _config;
        private readonly IGithubClient _github;
        private readonly INotificationService _notifications;

        public DeploymentService(IOptions<DeploymentConfiguration> config, IGithubClient github, INotificationService notifications)
        {
            _config = config.Value;
            _notifications = notifications;
            _github = github;

            github.Configure(_config.Repository, _config.ApiKey);
        }

        public string[] Environments => _config.Environments;

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

        public async Task DeployVersionAsync(string environment, string version)
        {
            try
            {
                await _github.WorkflowDispatchAsync(_config.Workflow, _config.Branch, new
                {
                    action = "deploy",
                    environment,
                    version
                });

                await _notifications.PushSuccessAsync($"Deployment for {environment} environment to {version} version started...");
            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync($"Unable to start deployment for {environment} environment to {version} version", exc);
                throw;
            }
        }

        public async Task RedeployAsync(string environment)
        {
            try
            {
                await _github.WorkflowDispatchAsync(_config.Workflow, _config.Branch, new
                {
                    action = "redeploy",
                    environment
                });

                await _notifications.PushSuccessAsync($"Redeployment for {environment} environment started...");
            }
            catch (Exception exc)
            {
                await _notifications.PushErrorAsync($"Unable to start Redeployment for {environment} environment", exc);
                throw;
            }
        }
    }
}
