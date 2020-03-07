using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Distvisor.Web.Services
{
    public interface IMailgunClient
    {
        string ApiKey { get; }
        string Domain { get; }

        void Configure(string domain, string apiKey);
        Task SendEmailAsync(MailgunEmail email);
    }

    public class MailgunClient : IMailgunClient
    {
        private readonly HttpClient _httpClient;

        public MailgunClient(HttpClient client)
        {
            _httpClient = client;
        }

        public string Domain { get; private set; }

        public string ApiKey { get; private set; }

        public void Configure(string domain, string apiKey)
        {
            Domain = domain;
            ApiKey = apiKey;

            var basicValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{apiKey}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicValue);
        }

        public async Task SendEmailAsync(MailgunEmail email)
        {
            EnsureConfigured();

            var url = new UriBuilder(_httpClient.BaseAddress);
            url.Path = $"v3/{Domain}/messages";
            
            var requestContent = new MultipartFormDataContent();
            requestContent.Add(new StringContent(email.From), "from");
            requestContent.Add(new StringContent(email.To), "to");
            requestContent.Add(new StringContent(email.Subject), "subject");
            requestContent.Add(new StringContent(email.Text), "text");

            if (email.Attachments.Any())
            {
                foreach (var attachment in email.Attachments)
                {
                    var fileContent = new ByteArrayContent(attachment.Bytes);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(attachment.ContentType);
                    requestContent.Add(fileContent, "attachment", attachment.FileName);
                }
            }

            var request = new HttpRequestMessage(HttpMethod.Post, url.ToString());
            request.Content = requestContent;
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        private void EnsureConfigured()
        {
            if (string.IsNullOrEmpty(Domain) || string.IsNullOrEmpty(ApiKey))
            {
                throw new InvalidOperationException("Mailgun client must be configured before first use");
            }
        }
    }

    public class MailgunEmail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }

        public IEnumerable<MailgunAttachment> Attachments { get; set; }
    }

    public class MailgunAttachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Bytes { get; set; }
    }
}
