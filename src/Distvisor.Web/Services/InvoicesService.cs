using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IInvoicesService
    {
        Task<byte[]> GetInvoicePdfAsync(string invoiceId);
        Task<IEnumerable<Invoice>> GetInvoicesAsync();
    }

    public class InvoicesService : IInvoicesService
    {
        private readonly RestClient _httpClient;

        public InvoicesService(IKeyVault keyVault)
        {
            _httpClient = new RestClient("https://www.ifirma.pl/");
            _httpClient.Authenticator = new InvoicesRestAuthenticator(keyVault);
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesAsync()
        {
            var request = new RestRequest("iapi/faktury.json", Method.GET);
            request.AddParameter("dataOd", "2019-10-01");
            request.AddParameter("dataDo", DateTime.Now.Date.ToString("yyyy-MM-dd"));

            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            var jobject = JObject.Parse(response.Content);
            var invoices = jobject.SelectToken("response.Wynik").Value<JArray>();
            var result = invoices
                .Select(x => x.Value<JObject>())
                .Select(x => new Invoice
                {
                    Id = x["FakturaId"].Value<string>(),
                    Number = "FV " + x["PelnyNumer"].Value<string>(),
                    Customer = x["IdentyfikatorKontrahenta"].Value<string>(),
                    Amount = x["Brutto"].Value<decimal>(),
                    IssueDate = DateTime.ParseExact(x["DataWystawienia"].Value<string>(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    WorkDays = 0,
                })
                .ToList();

            return result;
        }

        public async Task<byte[]> GetInvoicePdfAsync(string invoiceId)
        {
            var request = new RestRequest("iapi/fakturakraj/{invoiceId}.pdf.single", Method.GET);
            request.AddUrlSegment("invoiceId", invoiceId);
            request.AddHeader("Accept", "application/pdf");

            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            var result = response.RawBytes;

            return result;
        }
    }

    public class InvoicesRestAuthenticator : IAuthenticator
    {
        private readonly string _user;
        private readonly string _keyName;
        private readonly byte[] _keyBytes;

        public InvoicesRestAuthenticator(IKeyVault keyVault)
        {
            var key = keyVault.GetIFirmaApiKey().Result;
            _user = key.User;
            _keyName = "faktura";
            _keyBytes = HexStringToByteArray(key.Key);
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            var fullUrl = client.BuildUri(request);
            var url = fullUrl.AbsoluteUri;
            if (!string.IsNullOrEmpty(fullUrl.Query))
            {
                url = url.Replace(fullUrl.Query, "");
            }
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

    public class Invoice
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string Customer { get; set; }
        public DateTime IssueDate { get; set; }
        public int WorkDays { get; set; }
        public decimal Amount { get; set; }
    }
}
