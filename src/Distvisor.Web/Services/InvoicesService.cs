using Newtonsoft.Json.Linq;
using RestSharp;
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
        Task GenerateInvoiceAsync(string templateInvoiceId, DateTime issueDate, int quantity);
        Task<DateTime> GetActiveMonthAsync();
        Task<JObject> GetInvoiceJsonAsync(string invoiceId);
        Task<byte[]> GetInvoicePdfAsync(string invoiceId);
        Task<IEnumerable<Invoice>> GetInvoicesAsync();
    }

    public class InvoicesService : IInvoicesService
    {
        private readonly RestClient _httpClient;
        private readonly IKeyVault _keyVault;

        public InvoicesService(IKeyVault keyVault)
        {
            _httpClient = new RestClient("https://www.ifirma.pl/");
            _keyVault = keyVault;
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesAsync()
        {
            var request = new RestRequest("iapi/faktury.json", Method.GET);
            request.AddParameter("dataOd", "2019-10-01");
            request.AddParameter("dataDo", DateTime.Now.Date.ToString("yyyy-MM-dd"));
            request.AddHeader("Authentication", await GenerateAuthHeaderAsync(request));

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
            request.AddHeader("Authentication", await GenerateAuthHeaderAsync(request));

            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            var result = response.RawBytes;

            return result;
        }

        public async Task<DateTime> GetActiveMonthAsync()
        {
            var request = new RestRequest("iapi/abonent/miesiacksiegowy.json", Method.GET);
            request.AddHeader("Authentication", await GenerateAuthHeaderAsync(request));

            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            var content = JObject.Parse(response.Content);

            return DateTime.Now;
        }

        public async Task<JObject> GetInvoiceJsonAsync(string invoiceId)
        {
            var request = new RestRequest("iapi/fakturakraj/{invoiceId}.json", Method.GET);
            request.AddUrlSegment("invoiceId", invoiceId);
            request.AddHeader("Authentication", await GenerateAuthHeaderAsync(request));

            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            var content = JObject.Parse(response.Content);

            return content.Value<JObject>("response");
        }

        public async Task GenerateInvoiceAsync(string templateInvoiceId, DateTime issueDate, int quantity)
        {
            var templateInvoice = await GetInvoiceJsonAsync(templateInvoiceId);
            templateInvoice["Zaplacono"] = 0;
            templateInvoice["ZaplaconoNaDokumencie"] = 0;
            templateInvoice["NumerKontaBankowego"] = null;
            templateInvoice.Remove("IdentyfikatorKontrahenta");
            templateInvoice.Remove("PrefiksUEKontrahenta");
            templateInvoice.Remove("NIPKontrahenta");
            templateInvoice["DataWystawienia"] = issueDate.ToString("yyyy-MM-dd");
            templateInvoice["DataSprzedazy"] = issueDate.ToString("yyyy-MM-dd");
            templateInvoice["TerminPlatnosci"] = issueDate.AddDays(14).ToString("yyyy-MM-dd");
            templateInvoice["Numer"] = null;
            templateInvoice["Pozycje"][0]["Ilosc"] = quantity;
            templateInvoice["Pozycje"][0]["CenaZRabatem"].Parent.Remove();
            templateInvoice["Pozycje"][0]["StawkaRyczaltu"].Parent.Remove();
            templateInvoice["Pozycje"][0]["MagazynPozycjaId"].Parent.Remove();
            templateInvoice["Pozycje"][0]["MagazynMagazynId"].Parent.Remove();
            templateInvoice["Pozycje"][0]["MagazynObiektSprzedazyId"].Parent.Remove();

            var customerId = templateInvoice["Kontrahent"]["Identyfikator"].Value<string>();
            var customer = new JObject();
            customer["Identyfikator"] = customerId;

            templateInvoice["Kontrahent"] = customer;
            templateInvoice.Remove("PelnyNumer");

            var json = templateInvoice.ToString();
        }

        private async Task<string> GenerateAuthHeaderAsync(IRestRequest request)
        {
            var apiKey = await _keyVault.GetIFirmaApiKeyAsync();
            var user = apiKey.User;
            var keyName = "faktura";
            var keyBytes = Enumerable
                .Range(0, apiKey.Key.Length / 2)
                .Select(x => apiKey.Key.Substring(x * 2, 2))
                .Select(x => Convert.ToByte(x, 16))
                .ToArray();

            var fullUrl = _httpClient.BuildUri(request);
            var url = fullUrl.AbsoluteUri;
            if (!string.IsNullOrEmpty(fullUrl.Query))
            {
                url = url.Replace(fullUrl.Query, "");
            }
            var requestContent = request.Parameters
                .FirstOrDefault(p => p.Type == ParameterType.RequestBody);
            var stringToHash = $"{url}{user}{keyName}{requestContent}";
            var encoder = new System.Text.ASCIIEncoding();
            var textBytes = encoder.GetBytes(stringToHash);
            var hmacsha1 = new System.Security.Cryptography.HMACSHA1(keyBytes);
            byte[] hashCode = hmacsha1.ComputeHash(textBytes);
            string hash = BitConverter.ToString(hashCode).Replace("-", "").ToLower();
            string auth = $"IAPIS user={user}, hmac-sha1={hash}";

            return auth;
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
