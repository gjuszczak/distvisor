using Distvisor.Web.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IGithubService
    {
        Task<IEnumerable<string>> GetReleasesAsync();
        Task UpdateToVersionAsync(string version, string dbUpdateStrategy);
    }

    public class GithubService : IGithubService
    {
        private readonly EnvConfiguration _settings;
        private readonly RestClient _httpClient;

        public GithubService(IOptions<EnvConfiguration> settings)
        {
            _settings = settings.Value;
            _httpClient = new RestClient("https://api.github.com/");
        }

        public async Task<IEnumerable<string>> GetReleasesAsync()
        {
            var request = new RestRequest("repos/{owner}/{repo}/releases", Method.GET);
            request.AddUrlSegment("owner", _settings.GithubRepoOwner);
            request.AddUrlSegment("repo", _settings.GithubRepoName);
            request.AddHeader("User-Agent", _settings.GithubRepoOwner);
            request.AddHeader("Accept", "application/vnd.github.v3+json");

            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);

            var releases = JsonConvert.DeserializeObject<JArray>(response.Content);
            return releases.Select(r => r["tag_name"].Value<string>());
        }

        public async Task UpdateToVersionAsync(string version, string dbUpdateStrategy)
        {
            var request = new RestRequest("repos/{owner}/{repo}/dispatches", Method.POST);
            request.AddUrlSegment("owner", _settings.GithubRepoOwner);
            request.AddUrlSegment("repo", _settings.GithubRepoName);
            request.AddHeader("User-Agent", _settings.GithubRepoOwner);
            request.AddHeader("Accept", "application/vnd.github.everest-preview+json");
            request.AddHeader("Authorization", $"Bearer {_settings.GithubApiToken}");
            request.AddJsonBody(new
            {
                event_type = "deploy",
                client_payload = new
                {
                    from_version = _settings.CurrentVersion,
                    to_version = version,
                    db_update_strategy = dbUpdateStrategy,
                }
            });

            await _httpClient.ExecuteAsync(request, CancellationToken.None);
        }
    }
}
