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
        string Repository { get; }
        string ApiKey { get; }

        void Configure(string repository, string apiKey);
        Task<IEnumerable<string>> GetReleasesAsync();
        Task WorkflowDispatchAsync(string workflow, string reference, object inputs);
    }

    public class GithubClient : IGithubClient
    {
        private readonly HttpClient _httpClient;

        public GithubClient(HttpClient client)
        {
            _httpClient = client;
        }

        public string Repository { get; private set; }

        public string ApiKey { get; private set; }

        public void Configure(string repository, string apiKey)
        {
            Repository = repository;
            ApiKey = apiKey;
        }

        public async Task<IEnumerable<string>> GetReleasesAsync()
        {
            EnsureConfigured();

            var url = new UriBuilder(_httpClient.BaseAddress);
            url.Port = -1;
            url.Path = $"repos/{Repository}/releases";

            var request = new HttpRequestMessage(HttpMethod.Get, url.ToString());
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var releases = await JsonSerializer.DeserializeAsync<IEnumerable<GithubReleaseDto>>(responseStream);
            return releases.Select(r => r.TagName).ToList();
        }

        public async Task WorkflowDispatchAsync(string workflow, string reference, object inputs)
        {
            EnsureConfigured();

            var url = new UriBuilder(_httpClient.BaseAddress);
            url.Port = -1;
            url.Path = $"repos/{Repository}/actions/workflows/{workflow}/dispatches";

            var request = new HttpRequestMessage(HttpMethod.Post, url.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
            request.Content = new ByteArrayContent(JsonSerializer.SerializeToUtf8Bytes(new
            {
                @ref = reference,
                inputs
            }));
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        private void EnsureConfigured()
        {
            if (string.IsNullOrEmpty(Repository) || string.IsNullOrEmpty(ApiKey))
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
        public string Repository { get; }
        public void Configure(string repository, string apiKey)
        {
        }

        public Task<IEnumerable<string>> GetReleasesAsync()
        {
            return Task.FromResult((new string[] { "fake_v0.1", "fake_v0.2" }).AsEnumerable());
        }

        public async Task WorkflowDispatchAsync(string workflow, string reference, object inputs)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            await _notifications.PushFakeApiUsedAsync("github", new
            {
                workflow,
                reference,
                inputs,
            });
        }
    }
}
