using Newtonsoft.Json;
using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IMicrosoftOneDriveService
    {
        Task<MicrosoftUploadSession> CreateUploadSession(string filename);
    }

    public class MicrosoftOneDriveService : IMicrosoftOneDriveService
    {
        private readonly RestClient _httpClient;
        private readonly IMicrosoftAuthTokenStore _authTokenStore;

        public MicrosoftOneDriveService(IMicrosoftAuthTokenStore authTokenStore)
        {
            _httpClient = new RestClient("https://graph.microsoft.com/");
            _authTokenStore = authTokenStore;
        }

        public async Task<MicrosoftUploadSession> CreateUploadSession(string filename)
        {
            var token = await _authTokenStore.GetUserActiveToken();

            // make sure that approot is created before upload session request
            var request = new RestRequest("v1.0/me/drive/special/approot", Method.GET);
            request.AddHeader("Authorization", "Bearer " + token.AccessToken);
            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);

            request = new RestRequest($"v1.0/me/drive/special/approot:{filename}:/createUploadSession", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token.AccessToken);
            response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            var sessionInfo = JsonConvert.DeserializeObject<MicrosoftUploadSessionInfo>(response.Content);

            return new MicrosoftUploadSession(sessionInfo, _authTokenStore);
        }
    }
}
