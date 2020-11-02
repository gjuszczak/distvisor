using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Entities;
using Distvisor.Web.Data.Reads.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface ISecretsVault
    {
        string GetSecretValue(SecretKey key);
        Task<List<SecretKey>> ListSecretKeysAsync();
        Task RemoveSecretAsync(SecretKey key);
        Task SetSecretAsync(SecretKey key, string value);
    }

    public class SecretsVault : ISecretsVault
    {
        private readonly ReadStoreContext _context;
        private readonly IEventStore _eventStore;
        private readonly IMemoryCache _cache;

        public SecretsVault(ReadStoreContext context, IEventStore eventStore, IMemoryCache cache)
        {
            _context = context;
            _eventStore = eventStore;
            _cache = cache;
        }

        public async Task<List<SecretKey>> ListSecretKeysAsync()
        {
            return await _context.SecretsVault.Select(x => x.Key).ToListAsync();
        }

        public string GetSecretValue(SecretKey key)
        {
            if (!_cache.TryGetValue(key, out string secretValue))
            {
                var secretsVaultEntity = _context.SecretsVault.FirstOrDefault(x=> x.Key == key);

                if (!string.IsNullOrEmpty(secretsVaultEntity?.Value))
                {
                    var valueBytes = Convert.FromBase64String(secretsVaultEntity.Value);
                    secretValue = Encoding.UTF8.GetString(valueBytes); 
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.NeverRemove);

                _cache.Set(key, secretValue, cacheEntryOptions);
            }
            
            return secretValue;
        }

        public async Task RemoveSecretAsync(SecretKey key)
        {
            await _eventStore.Publish(new RemoveSecretEvent { Key = key });
            _cache.Remove(key);
        }

        public async Task SetSecretAsync(SecretKey key, string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            var entityValue = Convert.ToBase64String(valueBytes);

            await _eventStore.Publish(new SetSecretEvent { Key = key, Value = entityValue });

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetPriority(CacheItemPriority.NeverRemove);

            _cache.Set(key, value, cacheEntryOptions);
        }
    }

    public static class KeyVaultExtensions
    {
        public static AccountingSecrets GetAccountingSecrets(this ISecretsVault secretsVault)
        {
            return new AccountingSecrets
            {
                InvoicesApiKey = secretsVault.GetSecretValue(SecretKey.AccountingInvoicesApiKey),
                SubscriberApiKey = secretsVault.GetSecretValue(SecretKey.AccountingSubscriberApiKey),
                User = secretsVault.GetSecretValue(SecretKey.AccountingUser),
            };
        }
    }

    public class AccountingSecrets
    {
        public string InvoicesApiKey { get; set; }
        public string SubscriberApiKey { get; set; }
        public string User { get; set; }
    }
}
