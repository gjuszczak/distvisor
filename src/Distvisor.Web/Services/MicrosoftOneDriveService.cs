using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IMicrosoftOneDriveService
    {
        Task BackupDb();
        Task<MicrosoftUploadSession> CreateUploadSession(string filename);
    }

    public class MicrosoftOneDriveService : IMicrosoftOneDriveService
    {
        private readonly RestClient _httpClient;
        private readonly IMicrosoftAuthService _authService;

        public MicrosoftOneDriveService(IMicrosoftAuthService authService)
        {
            _httpClient = new RestClient("https://graph.microsoft.com/");
            _authService = authService;
        }

        public async Task<MicrosoftUploadSession> CreateUploadSession(string filename)
        {
            var token = await _authService.GetUserActiveTokenAsync();

            // make sure that approot is created before upload session request
            var request = new RestRequest("v1.0/me/drive/special/approot", Method.GET);
            request.AddHeader("Authorization", "Bearer " + token.AccessToken);
            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);

            request = new RestRequest($"v1.0/me/drive/special/approot:{filename}:/createUploadSession", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token.AccessToken);
            response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            var sessionInfo = JsonConvert.DeserializeObject<MicrosoftUploadSessionInfo>(response.Content);

            return new MicrosoftUploadSession(sessionInfo, _authService);
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
    }
}
