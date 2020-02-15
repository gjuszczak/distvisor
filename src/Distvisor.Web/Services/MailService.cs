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
        private readonly MailgunSecrets _secrets;

        public MailService(ISecretsVault keyVault)
        {
            _secrets = keyVault.GetMailgunSecrets();
            _httpClient = new RestClient("https://api.eu.mailgun.net/v3");
        }

        public async Task SendInvoicePdfAsync(byte[] invoicePdf)
        {
            var request = new RestRequest("{domain}/messages", Method.POST);
            request.AddUrlSegment("domain", _secrets.Domain);
            request.AddParameter("from", $"Distvisor <noreply@{_secrets.Domain}>");
            request.AddParameter("to", _secrets.ToAddress);
            request.AddParameter("subject", "Faktura od Distvisior");
            request.AddParameter("text", "Hejka. Faktura w zalaczniku.");
            request.AddFile("attachment", invoicePdf, "faktura.pdf", "application/pdf");
            request.Method = Method.POST;

            _httpClient.Authenticator = new HttpBasicAuthenticator("api", _secrets.ApiKey);
            var result = await _httpClient.ExecuteAsync(request, CancellationToken.None);
        }
    }
}
