using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IGithubService github;

        public SettingsController(IGithubService github)
        {
            this.github = github;
        }

        [HttpGet("updates")]
        public Task<IEnumerable<string>> GetUpdates()
        {
            return this.github.GetReleasesAsync();
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(string tag)
        {
            await this.github.UpdateToVersion(tag);
            return Ok($"Update to { tag } started.");
        }
    }
}