using RestSharp;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public class MicrosoftUploadSession : IDisposable
    {
        private const long BytesThrottle = 327680; //320 KiB

        private readonly MicrosoftUploadSessionInfo _sessionInfo;
        private readonly Func<Task<string>> _accessTokenProvider;
        private readonly RestClient _httpClient;

        public MicrosoftUploadSession(MicrosoftUploadSessionInfo sessionInfo, Func<Task<string>> accessTokenProvider)
        {
            _sessionInfo = sessionInfo;
            _accessTokenProvider = accessTokenProvider;
            _httpClient = new RestClient(_sessionInfo.UploadUrl);
        }

        public async Task Upload(string localFilePath)
        {
            var fileInfo = new FileInfo(localFilePath);
            if(!fileInfo.Exists)
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

            while (remainingBytes > 0)
            {
                var batchSize = (int)Math.Min(remainingBytes, BytesThrottle);
                stream.Read(buffer, 0, batchSize);
                                
                var accessToken = await _accessTokenProvider();
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Authorization", "Bearer " + accessToken);
                request.AddHeader("Content-Length", $"{batchSize}");
                request.AddHeader("Content-Range", $"bytes {offset}-{offset + batchSize - 1}/{bytes}");
                request.AddHeader("Content-Type", "application/binary");

                var body = buffer;
                if(body.Length > batchSize)
                {
                    body = new byte[batchSize];
                    Array.Copy(buffer, 0, body, 0, batchSize);
                }
                request.AddParameter("application/binary", body, ParameterType.RequestBody);

                var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
                if (response.IsSuccessful)
                {
                    remainingBytes -= batchSize;
                }
                else
                {
                    throw new Exception($"Failed to upload file. {response.StatusCode} {response.StatusDescription}");
                }
            }

        }

        public async Task Cancel()
        {
            var accessToken = await _accessTokenProvider();
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
        }

        public void Dispose()
        {
            Cancel().GetAwaiter().GetResult();
        }
    }

    public class MicrosoftUploadSessionInfo
    {
        public string UploadUrl { get; set; }
        public DateTime ExpirationDateTime { get; set; }
        public string[] NextExpectedRanges { get; set; }
    }
}
