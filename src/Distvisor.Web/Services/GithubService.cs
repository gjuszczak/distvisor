using Microsoft.Extensions.Options;
using Octokit;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IGithubService
    {
        Task<IEnumerable<string>> GetReleasesAsync();
        Task UpdateToVersion(string version);
    }

    public class GithubService : IGithubService
    {
        private readonly GithubSettings settings;

        public GithubService(IOptions<GithubSettings> settings)
        {
            this.settings = settings.Value;
        }

        public async Task<IEnumerable<string>> GetReleasesAsync()
        {
            var client = new GitHubClient(new ProductHeaderValue(settings.Repository));
            var releases = await client.Repository.Release.GetAll(settings.Owner, settings.Repository);
            return releases.Select(x => x.TagName).ToList();
        }

        public async Task UpdateToVersion(string version)
        {
            var client = new RestClient("https://api.github.com/");
            var request = new RestRequest("repos/{owner}/{repo}/dispatches", Method.POST);
            request.AddUrlSegment("owner", settings.Owner);
            request.AddUrlSegment("repo", settings.Repository);
            request.AddHeader("Accept", "application/vnd.github.everest-preview+json");
            request.AddHeader("Authorization", $"Bearer {settings.ApiToken}");
            request.AddJsonBody(new
            {
                event_type = "deploy",
                client_payload = new
                {
                    from_version = "todo",
                    to_version = version,
                }
            });

            await client.ExecuteTaskAsync(request);
        }
    }

    public class GithubSettings
    {
        public string ApiToken { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
    }
}
