using Newtonsoft.Json;
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
        Task SetActiveMonthAsync(DateTime date);
    }

    public class InvoicesService : IInvoicesService
    {
        private readonly DateTime _bottomDateLimit;
        private readonly RestClient _httpClient;
        private readonly AccountingSecrets _secrets;

        public InvoicesService(ISecretsVault keyVault)
        {
            _httpClient = new RestClient("https://www.ifirma.pl/");
            _secrets = keyVault.GetAccountingSecrets();
            _bottomDateLimit = new DateTime(2019, 10, 3);
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesAsync()
        {
            var request = new RestRequest("iapi/faktury.json", Method.GET);
            request.AddParameter("dataOd", _bottomDateLimit.ToString("yyyy-MM-dd"));
            request.AddParameter("dataDo", DateTime.Now.Date.ToString("yyyy-MM-dd"));
            request.AddHeader("Authentication", GenerateInvoiceAuthHeaderAsync(request));

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
            request.AddHeader("Authentication", GenerateInvoiceAuthHeaderAsync(request));

            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            var result = response.RawBytes;

            return result;
        }

        public async Task<DateTime> GetActiveMonthAsync()
        {
            var request = new RestRequest("iapi/abonent/miesiacksiegowy.json", Method.GET);
            request.AddHeader("Authentication", GenerateSubscriberAuthHeaderAsync(request));

            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            var content = JObject.Parse(response.Content);

            var year = content["response"].Value<int>("RokKsiegowy");
            var month = content["response"].Value<int>("MiesiacKsiegowy");
            var date = new DateTime(year, month, 1);

            return date;
        }

        public async Task SetActiveMonthAsync(DateTime date)
        {
            if (date < _bottomDateLimit)
            {
                throw new Exception($"Cannot set active month before {_bottomDateLimit.ToString("yyyy-MM-dd")}.");
            }

            var now = DateTime.Now;
            if (date.Year > now.Year || (date.Year == now.Year && date.Month > now.Month))
            {
                throw new Exception("Cannot set active month for future.");
            }

            var target = new DateTime(date.Year, date.Month, 1);
            var current = await GetActiveMonthAsync();
            var monthDiff = ((target.Year - current.Year) * 12) + target.Month - current.Month;
            if (monthDiff == 0)
            {
                return;
            }

            var goUp = monthDiff > 0;
            var absMonthDiff = Math.Abs(monthDiff);
            for (int i = 0; i < absMonthDiff; i++)
            {
                var request = new RestRequest("iapi/abonent/miesiacksiegowy.json", Method.PUT);
                request.AddJsonBody(new
                {
                    MiesiacKsiegowy = goUp ? "NAST" : "POPRZ",
                    PrzeniesDaneZPoprzedniegoRoku = true,
                });
                request.AddHeader("Authentication", GenerateSubscriberAuthHeaderAsync(request));

                var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            }
        }

        public async Task<JObject> GetInvoiceJsonAsync(string invoiceId)
        {
            var request = new RestRequest("iapi/fakturakraj/{invoiceId}.json", Method.GET);
            request.AddUrlSegment("invoiceId", invoiceId);
            request.AddHeader("Authentication", GenerateInvoiceAuthHeaderAsync(request));

            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
            var content = JObject.Parse(response.Content);

            return content.Value<JObject>("response");
        }

        public async Task GenerateInvoiceAsync(string templateInvoiceId, DateTime issueDate, int quantity)
        {
            if (issueDate < _bottomDateLimit)
            {
                throw new ArgumentException($"Cannot issue invoice before {_bottomDateLimit.ToString("yyyy-MM-dd")}.");
            }

            var now = DateTime.Now;
            if (issueDate.Year > now.Year || (issueDate.Year == now.Year && issueDate.Month > now.Month))
            {
                throw new Exception("Cannot issue invoice for future months.");
            }

            var templateInvoice = await GetInvoiceJsonAsync(templateInvoiceId);
            templateInvoice["Zaplacono"] = 0;
            templateInvoice["ZaplaconoNaDokumencie"] = 0;
            templateInvoice["NumerKontaBankowego"] = null;
            templateInvoice["PrefiksUEKontrahenta"].Parent.Remove();
            templateInvoice["NIPKontrahenta"].Parent.Remove();
            templateInvoice["DataWystawienia"] = issueDate.ToString("yyyy-MM-dd");
            templateInvoice["DataSprzedazy"] = issueDate.ToString("yyyy-MM-dd");
            templateInvoice["TerminPlatnosci"] = issueDate.AddDays(14).ToString("yyyy-MM-dd");
            templateInvoice["Numer"] = null;
            templateInvoice["Pozycje"][0]["Ilosc"] = quantity;
            templateInvoice["Pozycje"][0]["Id"].Parent.Remove();
            templateInvoice["Pozycje"][0]["CenaZRabatem"].Parent.Remove();
            templateInvoice["Pozycje"][0]["StawkaRyczaltu"].Parent.Remove();
            templateInvoice["Pozycje"][0]["MagazynPozycjaId"].Parent.Remove();
            templateInvoice["Pozycje"][0]["MagazynMagazynId"].Parent.Remove();
            templateInvoice["Pozycje"][0]["MagazynObiektSprzedazyId"].Parent.Remove();

            if (templateInvoice["IdentyfikatorKontrahenta"].Value<string>() == null)
            {
                templateInvoice["IdentyfikatorKontrahenta"] = templateInvoice["Kontrahent"]["Identyfikator"].Value<string>();
            }

            var json = templateInvoice.ToString();

            var request = new RestRequest("iapi/fakturakraj.json", Method.POST);
            request.AddJsonBody(JsonConvert.SerializeObject(templateInvoice));
            request.AddHeader("Authentication", GenerateInvoiceAuthHeaderAsync(request));

            var response = await _httpClient.ExecuteAsync(request, CancellationToken.None);
        }

        private string GenerateInvoiceAuthHeaderAsync(IRestRequest request)
        {
            return GenerateAuthHeaderAsync(request, "faktura", _secrets.InvoicesApiKey);
        }

        private string GenerateSubscriberAuthHeaderAsync(IRestRequest request)
        {
            return GenerateAuthHeaderAsync(request, "abonent", _secrets.SubscriberApiKey);
        }

        private string GenerateAuthHeaderAsync(IRestRequest request, string apiKeyName, string apiKeyValue)
        {
            var user = _secrets.User;
            var keyBytes = Enumerable
                .Range(0, apiKeyValue.Length / 2)
                .Select(x => apiKeyValue.Substring(x * 2, 2))
                .Select(x => Convert.ToByte(x, 16))
                .ToArray();

            var fullUrl = _httpClient.BuildUri(request);
            var url = fullUrl.AbsoluteUri;
            if (!string.IsNullOrEmpty(fullUrl.Query))
            {
                url = url.Replace(fullUrl.Query, "");
            }
            var requestContentValue = request.Parameters
                .FirstOrDefault(p => p.Type == ParameterType.RequestBody)?.Value;
            var requestContent = string.Empty;
            if (requestContentValue != null)
            {
                requestContent = requestContentValue as string ?? JsonConvert.SerializeObject(requestContentValue);
            }
            var stringToHash = $"{url}{user}{apiKeyName}{requestContent}";
            var encoder = new System.Text.UTF8Encoding();
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
