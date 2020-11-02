using Distvisor.Web.Data.Reads.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
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
        private readonly IMemoryCache _cache;
        private readonly ReadStoreContext _readStore;

        public EmailReceivingService(IMailgunClient mailgun, IMemoryCache cache, ReadStoreContext readStore)
        {
            _mailgun = mailgun;
            _cache = cache;
            _readStore = readStore;
        }

        public async IAsyncEnumerable<ReceivedEmail> PoolStoredEmailsAsync()
        {
            var storedEmails = await _mailgun.ListStoredEmailsAsync();
            foreach (var emailInfo in storedEmails)
            {
                if (await IsEmailEventAlreadyProcessed(emailInfo))
                {
                    continue;
                }

                var emailContent = await _mailgun.GetStoredEmailContentAsync(emailInfo.Url);
                yield return new ReceivedEmail
                {
                    Timestamp = emailInfo.Timestamp,
                    StorageKey = emailInfo.StorageKey,
                    BodyMime = emailContent
                };
            }
        }

        private async Task<bool> IsEmailEventAlreadyProcessed(MailgunStoredEvent emailReceivedEvent)
        {
            var key = $"{emailReceivedEvent.Timestamp}_{emailReceivedEvent.StorageKey}";
            if (!_cache.TryGetValue(key, out _))
            {
                var exists = await _readStore.ProcessedEmails.AnyAsync(x => x.UniqueKey == key);
                if (!exists)
                {
                    return false;
                }

                _cache.Set(key, true, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(2)
                });
            }

            return true;
        }
    }

    public class ReceivedEmail
    {
        public decimal Timestamp { get; set; }
        public string StorageKey { get; set; }
        public string BodyMime { get; set; }
    }
}
