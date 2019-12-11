using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private readonly GithubSettings _settings;
        private readonly RestClient _httpClient;

        public GithubService(IOptions<GithubSettings> settings)
        {
            _settings = settings.Value;
            _httpClient = new RestClient("https://api.github.com/");
        }

        public async Task<IEnumerable<string>> GetReleasesAsync()
        {
            var request = new RestRequest("repos/{owner}/{repo}/releases", Method.GET);
            request.AddUrlSegment("owner", _settings.Owner);
            request.AddUrlSegment("repo", _settings.Repository);
            request.AddHeader("User-Agent", _settings.Owner);
            request.AddHeader("Accept", "application/vnd.github.v3+json");

            var response = await _httpClient.ExecuteTaskAsync(request);

            var releases = JsonConvert.DeserializeObject<JArray>(response.Content);
            return releases.Select(r => r["tag_name"].Value<string>());
        }

        public async Task UpdateToVersion(string version)
        {
            var request = new RestRequest("repos/{owner}/{repo}/dispatches", Method.POST);
            request.AddUrlSegment("owner", _settings.Owner);
            request.AddUrlSegment("repo", _settings.Repository);
            request.AddHeader("User-Agent", _settings.Owner);
            request.AddHeader("Accept", "application/vnd.github.everest-preview+json");
            request.AddHeader("Authorization", $"Bearer {_settings.ApiToken}");
            request.AddJsonBody(new
            {
                event_type = "deploy",
                client_payload = new
                {
                    from_version = "todo",
                    to_version = version,
                }
            });

            await _httpClient.ExecuteTaskAsync(request);
        }
    }

    public class GithubSettings
    {
        public string ApiToken { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
    }
}
