using RestSharp;
using RestSharp.Authenticators;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IMailService
    {
        Task SendInvoicePdfAsync(byte[] invoicePdf);
    }

    public class MailService : IMailService
    {
        private readonly RestClient _httpClient;
        private readonly IKeyVault _keyVault;

        public MailService(IKeyVault keyVault)
        {
            _keyVault = keyVault;
            _httpClient = new RestClient("https://api.eu.mailgun.net/v3");
        }

        public async Task SendInvoicePdfAsync(byte[] invoicePdf)
        {
            var apiKey = await _keyVault.GetMailgunApiKeyAsync();
            var request = new RestRequest("{domain}/messages", Method.POST);
            request.AddUrlSegment("domain", apiKey.Domain);
            request.AddParameter("from", $"Distvisor <noreply@{apiKey.Domain}>");
            request.AddParameter("to", apiKey.To);
            request.AddParameter("subject", "Faktura od Distvisior");
            request.AddParameter("text", "Hejka. Faktura w zalaczniku.");
            request.AddFile("attachment", invoicePdf, "faktura.pdf", "application/pdf");
            request.Method = Method.POST;

            _httpClient.Authenticator = new HttpBasicAuthenticator("api", apiKey.Key);
            var result = await _httpClient.ExecuteAsync(request, CancellationToken.None);
        }
    }
}
