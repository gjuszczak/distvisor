using Distvisor.Web.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IMailgunClient
    {
        MailgunConfiguration Config { get; }
        Task SendEmailAsync(MailgunEmail email);
    }

    public class MailgunClient : IMailgunClient
    {
        private readonly HttpClient _httpClient;

        public MailgunClient(HttpClient client, IOptions<MailgunConfiguration> config)
        {
            _httpClient = client;
            Config = config.Value;

            var basicValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"api:{Config.ApiKey}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicValue);
        }

        public MailgunConfiguration Config { get; }

        public async Task SendEmailAsync(MailgunEmail email)
        {
            var url = new UriBuilder(_httpClient.BaseAddress)
            {
                Port = -1,
                Path = $"v3/{Config.Domain}/messages"
            };

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
    }

    public class FakeMailgunClient : IMailgunClient
    {
        private readonly INotificationService _notifications;

        public FakeMailgunClient(HttpClient _, INotificationService notifications)
        {
            _notifications = notifications;
        }

        public MailgunConfiguration Config => new MailgunConfiguration
        {
            ApiKey = "fakeApiKey",
            Domain = "fakeDomain",
        };

        public async Task SendEmailAsync(MailgunEmail email)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            await _notifications.PushFakeApiUsedAsync("mailgun", new
            {
                email.From,
                email.To,
                email.Subject,
                email.Text,
                Attachments = string.Join(", ", email.Attachments.Select(x => x.FileName))
            });
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
