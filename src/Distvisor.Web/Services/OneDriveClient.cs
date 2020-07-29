using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IOneDriveClient
    {
        Func<Task<string>> AccessTokenProvider { get; }

        void Configure(Func<Task<string>> accessTokenProvider);
        Task BackupDb();
        Task<IEnumerable<OneDriveFileInfo>> ListStoredBackupsAsync();
        Task<MicrosoftUploadSession> CreateUploadSession(string filename);
    }

    public class OneDriveClient : IOneDriveClient
    {
        private readonly HttpClient _httpClient;

        public OneDriveClient(HttpClient client)
        {
            _httpClient = client;
        }

        public Func<Task<string>> AccessTokenProvider { get; private set; }

        public void Configure(Func<Task<string>> accessTokenProvider)
        {
            AccessTokenProvider = accessTokenProvider;
        }

        public async Task<MicrosoftUploadSession> CreateUploadSession(string filename)
        {
            EnsureConfigured();

            var accessToken = await AccessTokenProvider();

            await EnsureAppRootIsReady(accessToken);

            var url = new UriBuilder(_httpClient.BaseAddress)
            {
                Port = -1,
                Path = $"v1.0/me/drive/special/approot:{filename}:/createUploadSession"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var sessionInfo = JsonConvert.DeserializeObject<MicrosoftUploadSessionInfo>(responseContent);

            return new MicrosoftUploadSession(sessionInfo, null);
        }

        public async Task<IEnumerable<OneDriveFileInfo>> ListStoredBackupsAsync()
        {
            var accessToken = await AccessTokenProvider();

            var url = new UriBuilder(_httpClient.BaseAddress)
            {
                Port = -1,
                Path = $"v1.0/me/drive/special/approot/children"
            };

            var request = new HttpRequestMessage(HttpMethod.Get, url.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var responseContentStream = await response.Content.ReadAsStreamAsync();
            using var responseContent = await JsonDocument.ParseAsync(responseContentStream);

            var files = responseContent.RootElement
                .GetProperty("value")
                .EnumerateArray()
                .Select(x => new OneDriveFileInfo
                {
                    Name = x.GetProperty("name").GetString(),
                    Size = x.GetProperty("size").GetInt64(),
                    CreatedDateTime = x.GetProperty("createdDateTime").GetDateTime(),
                    DownloadUrl = new Uri(x.GetProperty("@microsoft.graph.downloadUrl").GetString())
                })
                .ToList();

            return files ?? Enumerable.Empty<OneDriveFileInfo>();
        }

        public async Task BackupDb()
        {
            var databases = Directory.GetFiles("./Data", "*.db");
            foreach (var dbPath in databases)
            {
                var dbName = Path.GetFileName(dbPath);
                var dateString = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                using var session = await CreateUploadSession($"/backup_{dateString}_{dbName}");
                await session.Upload(dbPath);
            }
        }

        private async Task EnsureAppRootIsReady(string accessToken)
        {
            var url = new UriBuilder(_httpClient.BaseAddress)
            {
                Port = -1,
                Path = $"v1.0/me/drive/special/approot"
            };

            var request = new HttpRequestMessage(HttpMethod.Get, url.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        private void EnsureConfigured()
        {
            if (AccessTokenProvider == null)
            {
                throw new InvalidOperationException("OneDrive client must be configured before first use");
            }
        }
    }

    public class OneDriveFileInfo
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public Uri DownloadUrl { get; set; }
    }
}
