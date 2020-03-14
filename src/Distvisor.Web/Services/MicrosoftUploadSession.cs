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
        private readonly IMicrosoftAuthService _authService;
        private readonly RestClient _httpClient;

        public MicrosoftUploadSession(MicrosoftUploadSessionInfo sessionInfo, IMicrosoftAuthService authService)
        {
            _sessionInfo = sessionInfo;
            _authService = authService;
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
                var batchSize = (int)(remainingBytes > BytesThrottle ? BytesThrottle : remainingBytes);
                stream.Read(buffer, 0, batchSize);
                                
                var token = await _authService.GetUserActiveTokenAsync();
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Authorization", "Bearer " + token.AccessToken);
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
            }

        }

        public async Task Cancel()
        {
            var token = await _authService.GetUserActiveTokenAsync();

            var request = new RestRequest(Method.DELETE);
            request.AddHeader("Authorization", "Bearer " + token.AccessToken);
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
