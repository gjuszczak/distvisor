using Distvisor.App.Features.Backups.Queries.GetBackupFiles;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Infrastructure.Services.Admin
{
    public class FileHostingService : IFileHostingService
    {
        private readonly HttpClient _httpClient;
        private readonly IFileHostingAuthService _authService;

        public FileHostingService(HttpClient httpClient, IFileHostingAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        public async Task UploadAsync(string localFilePath, string remoteFileName, CancellationToken cancellationToken)
        {
            var session = await UploadSession.CreateAsync(_httpClient, _authService, remoteFileName, cancellationToken);
            try
            {
                await session.UploadAsync(localFilePath, cancellationToken);
            }
            finally
            {
                await session.CloseAsync(cancellationToken);
            }
        }

        public async Task DeleteAsync(string remoteFileName, CancellationToken cancellationToken)
        {
            var accessToken = await _authService.GetAccessTokenAsync(cancellationToken);

            var url = new UriBuilder(_httpClient.BaseAddress)
            {
                Port = -1,
                Path = $"v1.0/me/drive/special/approot:/{remoteFileName}:"
            };

            var request = new HttpRequestMessage(HttpMethod.Delete, url.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task RenameAsync(string oldRemoteFileName, string newRemoteFileName, CancellationToken cancellationToken)
        {
            var accessToken = await _authService.GetAccessTokenAsync(cancellationToken);

            var url = new UriBuilder(_httpClient.BaseAddress)
            {
                Port = -1,
                Path = $"v1.0/me/drive/special/approot:/{oldRemoteFileName}:"
            };

            var body = JsonSerializer.Serialize(new
            {
                name = newRemoteFileName
            });

            var request = new HttpRequestMessage(HttpMethod.Patch, url.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task<BackupFilesListDto> ListAsync(int offset, int batchSize, CancellationToken cancellationToken)
        {
            var accessToken = await _authService.GetAccessTokenAsync(cancellationToken);

            var urlBuilder = new UriBuilder(_httpClient.BaseAddress)
            {
                Port = -1,
                Path = $"v1.0/me/drive/special/approot/children"
            };

            var top = offset + batchSize;
            var queryParams = new Dictionary<string, string>
            {
                ["select"] = "name,size,createdDateTime",
                ["orderby"] = $"createdDateTime desc",
                ["top"] = $"{Math.Min(top, 200)}",
            };

            var url = QueryHelpers.AddQueryString(urlBuilder.ToString(), queryParams);
            var files = new List<BackupFileDto>();
            var totalCount = 0;
            while (!string.IsNullOrEmpty(url) && files.Count < top)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();

                using var responseContentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var responseContent = await JsonDocument.ParseAsync(responseContentStream, cancellationToken: cancellationToken);

                var batch = responseContent.RootElement
                    .GetProperty("value")
                    .EnumerateArray()
                    .Select(x => new BackupFileDto
                    {
                        Name = x.GetProperty("name").GetString(),
                        Size = x.GetProperty("size").GetInt64(),
                        CreatedDateTime = x.GetProperty("createdDateTime").GetDateTime()
                    })
                    .ToList();

                files.AddRange(batch);

                if (responseContent.RootElement.TryGetProperty("@odata.nextLink", out var nextLink))
                {
                    url = nextLink.GetString();
                }
                else
                {
                    url = null;
                }

                if (totalCount == 0 && responseContent.RootElement.TryGetProperty("@odata.count", out var count))
                {
                    totalCount = count.GetInt32();
                }
            }

            var dtos = files.Skip(offset).Take(batchSize).ToList();
            return new BackupFilesListDto(dtos, totalCount, offset, batchSize);
        }

        public async Task<TemporaryPath> DownloadAsync(string remoteFileName, CancellationToken cancellationToken)
        {
            var accessToken = await _authService.GetAccessTokenAsync(cancellationToken);
            var url = new UriBuilder(_httpClient.BaseAddress)
            {
                Port = -1,
                Path = $"v1.0/me/drive/special/approot:/{remoteFileName}:/content"
            };

            var request = new HttpRequestMessage(HttpMethod.Get, url.ToString());
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var tempFile = TemporaryPath.ForFile();
            using var downloadStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var fileStream = File.Create(tempFile.Path);
            await downloadStream.CopyToAsync(fileStream, cancellationToken);
            return tempFile;
        }

        private class UploadSession
        {
            private const long BytesThrottle = 327680; //320 KiB

            private readonly HttpClient _httpClient;
            private readonly IFileHostingAuthService _authService;
            private readonly UploadSessionInfo _sessionInfo;

            private UploadSession(HttpClient httpClient, UploadSessionInfo sessionInfo, IFileHostingAuthService authService)
            {
                _httpClient = httpClient;
                _sessionInfo = sessionInfo;
                _authService = authService;
            }

            public static async Task<UploadSession> CreateAsync(HttpClient httpClient, IFileHostingAuthService authService, string remoteFileName, CancellationToken cancellationToken)
            {
                await EnsureAppRootIsReadyAsync(httpClient, authService, cancellationToken);
                return await CreateSessionAsync(httpClient, authService, remoteFileName, cancellationToken);
            }

            private async static Task EnsureAppRootIsReadyAsync(HttpClient httpClient, IFileHostingAuthService authService, CancellationToken cancellationToken)
            {
                var accessToken = await authService.GetAccessTokenAsync(cancellationToken);

                var url = new UriBuilder(httpClient.BaseAddress)
                {
                    Port = -1,
                    Path = $"v1.0/me/drive/special/approot"
                };

                var request = new HttpRequestMessage(HttpMethod.Get, url.ToString());
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await httpClient.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();
            }

            private async static Task<UploadSession> CreateSessionAsync(HttpClient httpClient, IFileHostingAuthService authService, string remoteFileName, CancellationToken cancellationToken)
            {
                var accessToken = await authService.GetAccessTokenAsync(cancellationToken);

                var url = new UriBuilder(httpClient.BaseAddress)
                {
                    Port = -1,
                    Path = $"v1.0/me/drive/special/approot:/{remoteFileName}:/createUploadSession"
                };

                var request = new HttpRequestMessage(HttpMethod.Post, url.ToString());
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await httpClient.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var sessionInfo = await JsonSerializer.DeserializeAsync<UploadSessionInfo>(stream, cancellationToken: cancellationToken);

                return new UploadSession(httpClient, sessionInfo, authService);
            }

            public async Task UploadAsync(string localFilePath, CancellationToken cancellationToken)
            {
                var fileInfo = new FileInfo(localFilePath);
                if (!fileInfo.Exists)
                {
                    throw new FileNotFoundException($"File {localFilePath} does not exists");
                }

                using FileStream fs = File.OpenRead(fileInfo.FullName);
                await UploadAsync(fs, fileInfo.Length, cancellationToken);
            }

            public async Task UploadAsync(Stream stream, long bytes, CancellationToken cancellationToken)
            {
                var remainingBytes = bytes;
                var offset = 0;
                var buffer = new byte[BytesThrottle];

                var accessToken = await _authService.GetAccessTokenAsync(cancellationToken);
                while (remainingBytes > 0)
                {
                    var batchSize = (int)Math.Min(remainingBytes, BytesThrottle);
                    stream.Read(buffer, 0, batchSize);

                    var body = buffer;
                    if (body.Length > batchSize)
                    {
                        body = new byte[batchSize];
                        Array.Copy(buffer, 0, body, 0, batchSize);
                    }

                    var request = new HttpRequestMessage(HttpMethod.Put, _sessionInfo.UploadUrl);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    request.Content = new ByteArrayContent(body);
                    request.Content.Headers.ContentLength = batchSize;
                    request.Content.Headers.ContentRange = new ContentRangeHeaderValue(offset, offset + batchSize - 1, bytes);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/binary");

                    var response = await _httpClient.SendAsync(request, cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        remainingBytes -= batchSize;
                    }
                    else
                    {
                        throw new Exception($"Failed to upload file. {response.StatusCode} {response.ReasonPhrase}");
                    }
                }

            }

            public async Task CloseAsync(CancellationToken cancellationToken)
            {
                var accessToken = await _authService.GetAccessTokenAsync(cancellationToken);
                var request = new HttpRequestMessage(HttpMethod.Delete, _sessionInfo.UploadUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                await _httpClient.SendAsync(request, cancellationToken);
            }

            public class UploadSessionInfo
            {
                [JsonPropertyName("uploadUrl")]
                public string UploadUrl { get; set; }

                [JsonPropertyName("expirationDateTime")]
                public DateTime ExpirationDateTime { get; set; }

                [JsonPropertyName("nextExpectedRanges")]
                public string[] NextExpectedRanges { get; set; }
            }
        }
    }
}
