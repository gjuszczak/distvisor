using Distvisor.Web.Data.Entities;
using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class KeyVaultController : ControllerBase
    {
        private readonly IKeyVault _keyVault;

        public KeyVaultController(IKeyVault keyVault)
        {
            _keyVault = keyVault;
        }

        [HttpGet("list")]
        public Task<List<KeyType>> ListKeys()
        {
            return _keyVault.ListAvailableKeys();
        }

        [HttpDelete("{keyType}")]
        public async Task<IActionResult> RemoveKey(KeyType keyType)
        {
            await _keyVault.RemoveKey(keyType);
            return Ok();
        }

        [HttpPost("{keyType}")]
        public async Task<IActionResult> SetKey(KeyType keyType, [FromBody]dynamic body)
        {
            await _keyVault.SetKey(keyType, body.ToString());
            return Ok();
        }
    }
}
