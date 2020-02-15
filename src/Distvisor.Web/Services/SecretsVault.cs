using Distvisor.Web.Data;
using Distvisor.Web.Data.Entities;
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
        private readonly DistvisorContext _context;
        private readonly IMemoryCache _cache;

        public SecretsVault(DistvisorContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public Task<List<SecretKey>> ListSecretKeysAsync()
        {
            return _context.SecretsVault.Select(x => x.Key).ToListAsync();
        }

        public string GetSecretValue(SecretKey key)
        {
            if (!_cache.TryGetValue(key, out string secretValue))
            {
                var secretsVaultEntity = _context.SecretsVault.Find(key);

                if (!string.IsNullOrEmpty(secretsVaultEntity?.Value))
                {
                    var valueBytes = Convert.FromBase64String(secretsVaultEntity.Value);
                    secretValue = Encoding.UTF8.GetString(valueBytes); 

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetPriority(CacheItemPriority.NeverRemove);

                    _cache.Set(key, secretValue, cacheEntryOptions);
                }
            }
            
            return secretValue;
        }

        public async Task RemoveSecretAsync(SecretKey key)
        {
            SecretsVaultEntity e = new SecretsVaultEntity() { Key = key };
            _context.SecretsVault.Attach(e);
            _context.SecretsVault.Remove(e);
            await _context.SaveChangesAsync();

            _cache.Remove(key);
        }

        public async Task SetSecretAsync(SecretKey key, string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            var entityValue = Convert.ToBase64String(valueBytes);

            var secretsVaultEntity = await _context.SecretsVault.FindAsync(key);
            if (secretsVaultEntity == null)
            {
                secretsVaultEntity = new SecretsVaultEntity { Key = key };
                _context.SecretsVault.Add(secretsVaultEntity);
            }

            secretsVaultEntity.Value = entityValue;

            await _context.SaveChangesAsync();

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
                RepoName = secretsVault.GetSecretValue(SecretKey.GithubRepoOwner),
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
}
