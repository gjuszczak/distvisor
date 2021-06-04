using Distvisor.Web.Data.Reads.Entities;
using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/sec/[controller]")]
    public class SecretsVaultController : ControllerBase
    {
        private readonly ISecretsVault _secretsVault;

        public SecretsVaultController(ISecretsVault secretsVault)
        {
            _secretsVault = secretsVault;
        }

        [HttpGet("list")]
        public async Task<List<SecretKey>> SecretKeys()
        {
            return await _secretsVault.ListSecretKeysAsync();
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> RemoveSecretKey(SecretKey key)
        {
            await _secretsVault.RemoveSecretAsync(key);
            return Ok();
        }

        [HttpPost("{key}")]
        public async Task<IActionResult> SetSecret(SecretKey key, string value)
        {
            await _secretsVault.SetSecretAsync(key, value);
            return Ok();
        }
    }
}
