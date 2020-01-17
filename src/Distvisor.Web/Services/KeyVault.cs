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
        Task<JObject> GetKey(KeyType keyType);
        Task<List<KeyType>> ListAvailableKeys();
        Task RemoveKey(KeyType keyType);
        Task SetKey(KeyType keyType, string keyValue);
    }

    public class KeyVault : IKeyVault
    {
        private readonly DistvisorContext _context;

        public KeyVault(DistvisorContext context)
        {
            _context = context;
        }

        public Task<List<KeyType>> ListAvailableKeys()
        {
            return _context.KeyVault.Select(x => x.Id).ToListAsync();
        }

        public async Task<JObject> GetKey(KeyType keyType)
        {
            var key = await _context.KeyVault.FindAsync(keyType);
            if (string.IsNullOrEmpty(key?.KeyValue))
            {
                return null;
            }

            var byteValue = Convert.FromBase64String(key.KeyValue);
            var jsonValue = Encoding.UTF8.GetString(byteValue);
            var jobjectValue = JObject.Parse(jsonValue);
            return jobjectValue;
        }

        public Task RemoveKey(KeyType keyType)
        {
            KeyVaultEntity e = new KeyVaultEntity() { Id = keyType };
            _context.KeyVault.Attach(e);
            _context.KeyVault.Remove(e);
            return _context.SaveChangesAsync();
        }

        public async Task SetKey(KeyType keyType, string keyValue)
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
        public static async Task<IFirmaApiKey> GetIFirmaApiKey(this IKeyVault keyVault)
        {
            var keyData = await keyVault.GetKey(KeyType.IFirmaApiKey);
            var key = new IFirmaApiKey();
            JsonConvert.PopulateObject(keyData.ToString(), key);
            return key;
        }
    }

    public class IFirmaApiKey
    {
        public string Key { get; set; }
        public string User { get; set; }
    }
}
