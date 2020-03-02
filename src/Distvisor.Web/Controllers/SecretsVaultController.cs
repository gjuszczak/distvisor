using Distvisor.Web.Data.Entities;
using Distvisor.Web.Hubs;
using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SecretsVaultController : ControllerBase
    {
        private readonly ISecretsVault _secretsVault;
        private readonly IHubContext<NotificationsHub> _hub;

        public SecretsVaultController(ISecretsVault secretsVault, IHubContext<NotificationsHub> hub)
        {
            _secretsVault = secretsVault;
            _hub = hub;
        }

        [HttpGet("list")]
        public async Task<List<SecretKey>> SecretKeys()
        {
            await _hub.Clients.All.SendAsync("ReceiveMessage", "user", "message");
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
