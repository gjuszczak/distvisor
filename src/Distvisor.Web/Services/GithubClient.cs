using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IGithubClient
    {
        string ApiKey { get; }
        string RepoName { get; }
        string RepoOwner { get; }

        void Configure(string repoOwner, string repoName, string apiKey);
        Task<IEnumerable<string>> GetReleasesAsync();
        Task UpdateToVersionAsync(string fromVersion, string toVersion, string dbUpdateStrategy);
    }

    public class GithubClient : IGithubClient
    {
        private readonly HttpClient _httpClient;

        public GithubClient(HttpClient client)
        {
            _httpClient = client;
        }

        public string RepoOwner { get; private set; }

        public string RepoName { get; private set; }

        public string ApiKey { get; private set; }

        public void Configure(string repoOwner, string repoName, string apiKey)
        {
            RepoOwner = repoOwner;
            RepoName = repoName;
            ApiKey = apiKey;
        }

        public async Task<IEnumerable<string>> GetReleasesAsync()
        {
            EnsureConfigured();

            var url = new UriBuilder(_httpClient.BaseAddress);
            url.Port = -1;
            url.Path = $"repos/{RepoOwner}/{RepoName}/releases";

            var request = new HttpRequestMessage(HttpMethod.Get, url.ToString());
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var releases = await JsonSerializer.DeserializeAsync<IEnumerable<GithubReleaseDto>>(responseStream);
            return releases.Select(r => r.TagName).ToList();
        }

        public async Task UpdateToVersionAsync(string fromVersion, string toVersion, string dbUpdateStrategy)
        {
            EnsureConfigured();

            var url = new UriBuilder(_httpClient.BaseAddress);
            url.Port = -1;
            url.Path = $"repos/{RepoOwner}/{RepoName}/dispatches";

            var request = new HttpRequestMessage(HttpMethod.Post, url.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
            request.Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(new
            {
                event_type = "deploy",
                client_payload = new
                {
                    from_version = fromVersion,
                    to_version = toVersion,
                    db_update_strategy = dbUpdateStrategy,
                }
            }));
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        private void EnsureConfigured()
        {
            if (string.IsNullOrEmpty(RepoOwner) || string.IsNullOrEmpty(RepoName) || string.IsNullOrEmpty(ApiKey))
            {
                throw new InvalidOperationException("Github client must be configured before first use");
            }
        }

        private class GithubReleaseDto
        {
            [JsonPropertyName("tag_name")]
            public string TagName { get; set; }
        }
    }

    public class FakeGithubClient : IGithubClient
    {
        private readonly INotificationService _notifications;

        public FakeGithubClient(HttpClient _, INotificationService notifications)
        {
            _notifications = notifications;
        }
        public string ApiKey { get; }
        public string RepoName { get; }
        public string RepoOwner { get; }
        public void Configure(string repoOwner, string repoName, string apiKey)
        {
        }

        public Task<IEnumerable<string>> GetReleasesAsync()
        {
            return Task.FromResult((new string[] { "fake_v0.1", "fake_v0.2" }).AsEnumerable());
        }

        public async Task UpdateToVersionAsync(string fromVersion, string toVersion, string dbUpdateStrategy)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            await _notifications.PushFakeApiUsedAsync("github", new
            {
                fromVersion,
                toVersion,
                dbUpdateStrategy,
            });
        }
    }
}
