using Distvisor.Web.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Distvisor.Web.Services
{
    public interface IMailgunClient
    {
        MailgunConfiguration Config { get; }
        Task SendEmailAsync(MailgunEmail email);
        Task SendEmailAsync(MailgunTemplateEmail email);
        Task<IEnumerable<MailgunStoredEvent>> ListStoredEmailsAsync();
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

        public async Task SendEmailAsync(MailgunTemplateEmail email)
        {
            var url = new UriBuilder(_httpClient.BaseAddress)
            {
                Port = -1,
                Path = $"v3/{Config.Domain}/messages"
            };

            var jsonVariables = JsonSerializer.Serialize(email.Variables);

            var requestContent = new MultipartFormDataContent();
            requestContent.Add(new StringContent(email.From), "from");
            requestContent.Add(new StringContent(email.To), "to");
            requestContent.Add(new StringContent(email.Subject), "subject");
            requestContent.Add(new StringContent(email.Template), "template");
            requestContent.Add(new StringContent(jsonVariables), "h:X-Mailgun-Variables");

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

        public async Task<IEnumerable<MailgunStoredEvent>> ListStoredEmailsAsync()
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query.Add("begin", DateTimeOffset.Now.AddDays(-2).ToUnixTimeSeconds().ToString());
            query.Add("end", DateTimeOffset.Now.ToUnixTimeSeconds().ToString());
            query.Add("event", "stored");

            var url = new UriBuilder(_httpClient.BaseAddress)
            {
                Port = -1,
                Path = $"v3/{Config.Domain}/events",
                Query = query.ToString()
            };

            var request = new HttpRequestMessage(HttpMethod.Get, url.ToString());
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();


            var stream = await response.Content.ReadAsStreamAsync();
            var content = await JsonDocument.ParseAsync(stream);

            var result = content.RootElement.GetProperty("items").EnumerateArray().Select(item => new MailgunStoredEvent()
            {
                Timestamp = item.GetProperty("timestamp").GetDecimal(),
                StorageKey = item.GetProperty("storage").GetProperty("key").GetString()
            }).ToArray();

            return result;
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

        public Task<IEnumerable<MailgunStoredEvent>> ListStoredEmailsAsync()
        {
            return Task.FromResult((IEnumerable<MailgunStoredEvent>)new MailgunStoredEvent[0]);
        }

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

        public async Task SendEmailAsync(MailgunTemplateEmail email)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));

            await _notifications.PushFakeApiUsedAsync("mailgun", new
            {
                email.From,
                email.To,
                email.Template,
                email.Variables,
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

    public class MailgunTemplateEmail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Template { get; set; }
        public object Variables { get; set; }

        public IEnumerable<MailgunAttachment> Attachments { get; set; }
    }

    public class MailgunAttachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Bytes { get; set; }
    }

    public class MailgunStoredEvent
    {
        public decimal Timestamp { get; set; }
        public string StorageKey { get; set; }
    }
}
