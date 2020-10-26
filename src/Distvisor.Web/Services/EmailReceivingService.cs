using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IEmailReceivingService
    {
        IAsyncEnumerable<ReceivedEmail> PoolStoredEmailsAsync();
    }

    public class EmailReceivingService : IEmailReceivingService
    {
        private readonly IMailgunClient _mailgun;

        public EmailReceivingService(IMailgunClient mailgun)
        {
            _mailgun = mailgun;
        }

        public async IAsyncEnumerable<ReceivedEmail> PoolStoredEmailsAsync()
        {
            var storedEmails = await _mailgun.ListStoredEmailsAsync();
            foreach (var emailInfo in storedEmails)
            {
                var emailContent = await _mailgun.GetStoredEmailContentAsync(emailInfo.Url);
                yield return new ReceivedEmail
                {
                    Timestamp = emailInfo.Timestamp,
                    StorageKey = emailInfo.StorageKey,
                    BodyMime = emailContent
                };
            }
        }
    }

    public class ReceivedEmail
    {
        public decimal Timestamp { get; set; }
        public string StorageKey { get; set; }
        public string BodyMime { get; set; }
    }
}
