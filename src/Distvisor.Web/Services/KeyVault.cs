using Distvisor.Web.Data;
using Distvisor.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IKeyVault
    {
        Task<T> GetKeyAsync<T>(KeyType keyType) where T : class;
        Task<List<KeyType>> ListAvailableKeysAsync();
        Task RemoveKeyAsync(KeyType keyType);
        Task SetKeyAsync(KeyType keyType, string keyValue);
    }

    public class KeyVault : IKeyVault
    {
        private readonly DistvisorContext _context;

        public KeyVault(DistvisorContext context)
        {
            _context = context;
        }

        public Task<List<KeyType>> ListAvailableKeysAsync()
        {
            return _context.KeyVault.Select(x => x.Id).ToListAsync();
        }

        public async Task<T> GetKeyAsync<T>(KeyType keyType) where T : class
        {
            var key = await _context.KeyVault.FindAsync(keyType);
            if (string.IsNullOrEmpty(key?.KeyValue))
            {
                return null;
            }
            var byteValue = Convert.FromBase64String(key.KeyValue);
            var jsonValue = Encoding.UTF8.GetString(byteValue);
            return JsonConvert.DeserializeObject<T>(jsonValue);
        }

        public Task RemoveKeyAsync(KeyType keyType)
        {
            KeyVaultEntity e = new KeyVaultEntity() { Id = keyType };
            _context.KeyVault.Attach(e);
            _context.KeyVault.Remove(e);
            return _context.SaveChangesAsync();
        }

        public async Task SetKeyAsync(KeyType keyType, string keyValue)
        {
            var jsonBody = JObject.Parse(keyValue).ToString(Formatting.None);
            var jsonBytes = Encoding.UTF8.GetBytes(jsonBody);
            var base64Body = Convert.ToBase64String(jsonBytes);

            var key = await _context.KeyVault.FindAsync(keyType);
            if (key == null)
            {
                key = new KeyVaultEntity { Id = keyType };
                _context.KeyVault.Add(key);
            }

            key.KeyValue = base64Body;

            await _context.SaveChangesAsync();
        }
    }

    public static class KeyVaultExtensions
    {
        public static Task<IFirmaApiKey> GetIFirmaApiKeyAsync(this IKeyVault keyVault)
        {
            return keyVault.GetKeyAsync<IFirmaApiKey>(KeyType.IFirmaApiKey);
        }

        public static Task<MailgunApiKey> GetMailgunApiKeyAsync(this IKeyVault keyVault)
        {
            return keyVault.GetKeyAsync<MailgunApiKey>(KeyType.MailgunApiKey);
        }

        public static Task<GithubApiKey> GetGithubApiKeyAsync(this IKeyVault keyVault)
        {
            return keyVault.GetKeyAsync<GithubApiKey>(KeyType.GithubApiKey);
        }
    }

    public class IFirmaApiKey
    {
        public string InvoiceKey { get; set; }
        public string SubscriberKey { get; set; }
        public string User { get; set; }
    }

    public class MailgunApiKey
    {
        public string Key { get; set; }
        public string Domain { get; set; }
        public string To { get; set; }
    }

    public class GithubApiKey
    {
        public string Key { get; set; }
        public string RepoOwner { get; set; }
        public string RepoName { get; set; }
    }
}
