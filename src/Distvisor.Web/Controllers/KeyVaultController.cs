using Distvisor.Web.Data;
using Distvisor.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class KeyVaultController : ControllerBase
    {
        private readonly DistvisorContext _context;

        public KeyVaultController(DistvisorContext context)
        {
            _context = context;
        }

        [HttpGet("list")]
        public Task<List<KeyType>> ListKeys()
        {
            return _context.KeyVault.Select(x => x.Id).ToListAsync();
        }

        [HttpGet("{keyType}")]
        public async Task<ActionResult<dynamic>> GetKey(KeyType keyType)
        {
            var key = await _context.KeyVault.FindAsync(keyType);
            if (key == null)
            {
                return BadRequest();
            }

            var jsonValue = Encoding.UTF8.GetString(Convert.FromBase64String(key.KeyValue));
            var jobjectValue = JObject.Parse(jsonValue);
            return jobjectValue;
        }

        [HttpDelete("{keyType}")]
        public async Task<IActionResult> RemoveKey(KeyType keyType)
        {
            KeyVaultEntity e = new KeyVaultEntity() { Id = keyType };
            _context.KeyVault.Attach(e);
            _context.KeyVault.Remove(e);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("{keyType}")]
        public async Task<IActionResult> SaveKey(KeyType keyType, [FromBody]dynamic body)
        {
            var jsonBody = JObject.FromObject(body).ToString(Formatting.None);
            var base64Body = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonBody));

            var key = await _context.KeyVault.FindAsync(keyType);
            if (key == null)
            {
                key = new KeyVaultEntity { Id = keyType };
                _context.KeyVault.Add(key);
            }

            key.KeyValue = base64Body;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    public enum KeyType
    {
        GithubApiKey,
        IFirmaApiKey,
    }
}
