using Distvisor.Web.Data.Entities;
using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads;
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
        private readonly ReadStore _context;
        private readonly IEventStore _eventStore;
        private readonly IMemoryCache _cache;

        public SecretsVault(ReadStore context, IEventStore eventStore, IMemoryCache cache)
        {
            _context = context;
            _eventStore = eventStore;
            _cache = cache;
        }

        public async Task<List<SecretKey>> ListSecretKeysAsync()
        {
            return _context.SecretsVault.FindAll().Select(x => x.Key).ToList();
        }

        public string GetSecretValue(SecretKey key)
        {
            if (!_cache.TryGetValue(key, out string secretValue))
            {
                var secretsVaultEntity = _context.SecretsVault.FindOne(x=> x.Key == key);

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
            _eventStore.Publish(new RemoveSecretEvent { Key = key });
            _cache.Remove(key);
        }

        public async Task SetSecretAsync(SecretKey key, string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            var entityValue = Convert.ToBase64String(valueBytes);

            _eventStore.Publish(new SetSecretEvent { Key = key, Value = entityValue });

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

        public static MailgunSecrets GetMailgunSecrets(this ISecretsVault secretsVault)
        {
            return new MailgunSecrets
            {
                ApiKey = secretsVault.GetSecretValue(SecretKey.MailgunApiKey),
                Domain = secretsVault.GetSecretValue(SecretKey.MailgunDomain),
                ToAddress = secretsVault.GetSecretValue(SecretKey.MailgunToAddress),
            };
        }

        public static GithubSecrets GetGithubSecrets(this ISecretsVault secretsVault)
        {
            return new GithubSecrets
            {
                ApiKey = secretsVault.GetSecretValue(SecretKey.GithubApiKey),
                RepoOwner = secretsVault.GetSecretValue(SecretKey.GithubRepoOwner),
                RepoName = secretsVault.GetSecretValue(SecretKey.GithubRepoName),
            };
        }

        public static MicrosoftSecrets GetMicrosoftSecrets(this ISecretsVault secretsVault)
        {
            return new MicrosoftSecrets
            {
                AppClientId = secretsVault.GetSecretValue(SecretKey.MicrosoftAppClientId),
                AppSecret = secretsVault.GetSecretValue(SecretKey.MicrosoftAppSecret),
                AuthRedirectUri = secretsVault.GetSecretValue(SecretKey.MicrosoftAuthRedirectUri),
            };
        }
    }

    public class AccountingSecrets
    {
        public string InvoicesApiKey { get; set; }
        public string SubscriberApiKey { get; set; }
        public string User { get; set; }
    }

    public class MailgunSecrets
    {
        public string ApiKey { get; set; }
        public string Domain { get; set; }
        public string ToAddress { get; set; }
    }

    public class GithubSecrets
    {
        public string ApiKey { get; set; }
        public string RepoOwner { get; set; }
        public string RepoName { get; set; }
    }

    public class MicrosoftSecrets
    {
        public string AppClientId { get; set; }
        public string AppSecret { get; set; }
        public string AuthRedirectUri { get; set; }
    }
}
