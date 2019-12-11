using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IInvoicesService
    {
        Task<IEnumerable<string>> GetInvoicesAsync();
    }

    public class InvoicesService : IInvoicesService
    {
        private readonly GithubSettings _settings;
        private readonly RestClient _httpClient;

        public InvoicesService(IOptions<GithubSettings> settings)
        {
            _settings = settings.Value;
            _httpClient = new RestClient("https://www.ifirma.pl/");
            //_httpClient.Authenticator = new InvoicesRestAuthenticator(...);
        }

        public Task<IEnumerable<string>> GetInvoicesAsync()
        {
            //var request = new RestRequest("iapi/faktury.json", Method.GET);
            //request.AddParameter("dataOd", "2019-10-03");
            //request.AddParameter("dataDo", "2019-12-10");

            //var response = await _httpClient.ExecuteTaskAsync(request);

            var result = new[] { "Test1", "Test2" };
            return Task.FromResult((IEnumerable<string>)result);
        }
    }

    public class InvoicesSettings
    {
        //public string ApiToken { get; set; }
        //public string Owner { get; set; }
        //public string Repository { get; set; }
    }

    public class InvoicesRestAuthenticator : IAuthenticator
    {
        private readonly string _user;
        private readonly string _keyName;
        private readonly byte[] _keyBytes;

        public InvoicesRestAuthenticator(string user, string keyName, string key)
        {
            _user = user;
            _keyName = keyName;
            _keyBytes = HexStringToByteArray(key);
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            var fullUrl = client.BuildUri(request);
            var url = fullUrl.AbsoluteUri.Replace(fullUrl.Query, "");
            var requestContent = request.Parameters
                .FirstOrDefault(p => p.Type == ParameterType.RequestBody);
            var stringToHash = $"{url}{_user}{_keyName}{requestContent}";
            var encoder = new System.Text.ASCIIEncoding();
            var textBytes = encoder.GetBytes(stringToHash);
            var hmacsha1 = new System.Security.Cryptography.HMACSHA1(_keyBytes);
            byte[] hashCode = hmacsha1.ComputeHash(textBytes);
            string hash = System.BitConverter.ToString(hashCode).Replace("-", "").ToLower();
            string auth = $"IAPIS user={_user}, hmac-sha1={hash}";

            request.AddHeader("Authentication", auth);
        }

        private byte[] HexStringToByteArray(string hexString)
        {
            return Enumerable
                .Range(0, hexString.Length / 2)
                .Select(x => hexString.Substring(x * 2, 2))
                .Select(x => System.Convert.ToByte(x, 16))
                .ToArray();
        }
    }
}
