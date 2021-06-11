using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IOneDriveUploadSessionFactory
    {
        OneDriveUploadSessionClient CreateUploadSession(MicrosoftUploadSessionInfo sessionInfo, Func<Task<string>> accessTokenProvider);
    }

    public class OneDriveUploadSessionFactory : IOneDriveUploadSessionFactory
    {
        private readonly HttpClient _httpClient;

        public OneDriveUploadSessionFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public OneDriveUploadSessionClient CreateUploadSession(MicrosoftUploadSessionInfo sessionInfo, Func<Task<string>> accessTokenProvider)
        {
            return new OneDriveUploadSessionClient(_httpClient, sessionInfo, accessTokenProvider);
        }
    }

    public class OneDriveUploadSessionClient : IDisposable
    {
        private const long BytesThrottle = 327680; //320 KiB

        private readonly HttpClient _httpClient;
        private readonly MicrosoftUploadSessionInfo _sessionInfo;
        private readonly Func<Task<string>> _accessTokenProvider;

        public OneDriveUploadSessionClient(HttpClient httpClient, MicrosoftUploadSessionInfo sessionInfo, Func<Task<string>> accessTokenProvider)
        {
            _httpClient = httpClient;
            _sessionInfo = sessionInfo;
            _accessTokenProvider = accessTokenProvider;
        }

        public async Task Upload(string localFilePath)
        {
            var fileInfo = new FileInfo(localFilePath);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException($"File {localFilePath} does not exists");
            }

            using FileStream fs = File.OpenRead(fileInfo.FullName);
            await Upload(fs, fileInfo.Length);
        }

        public async Task Upload(Stream stream, long bytes)
        {
            var remainingBytes = bytes;
            var offset = 0;
            var buffer = new byte[BytesThrottle];

            var accessToken = await _accessTokenProvider();
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

                var response = await _httpClient.SendAsync(request, CancellationToken.None);
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

        public async Task Cancel()
        {
            var accessToken = await _accessTokenProvider();
            var request = new HttpRequestMessage(HttpMethod.Delete, _sessionInfo.UploadUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            await _httpClient.SendAsync(request, CancellationToken.None);
        }

        public void Dispose()
        {
            Cancel().GetAwaiter().GetResult();
        }
    }

    public class MicrosoftUploadSessionInfo
    {
        [JsonPropertyName("uploadUrl")]
        public string UploadUrl { get; set; }

        [JsonPropertyName("expirationDateTime")]
        public DateTime ExpirationDateTime { get; set; }

        [JsonPropertyName("nextExpectedRanges")]
        public string[] NextExpectedRanges { get; set; }
    }
}
