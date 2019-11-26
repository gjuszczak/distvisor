using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IDockerService _docker;
        private readonly IGithubService _github;
        private readonly IHostApplicationLifetime _appHost;

        public SettingsController(IDockerService docker, IGithubService github, IHostApplicationLifetime appHost)
        {
            _docker = docker;
            _github = github;
            _appHost = appHost;
        }

        [HttpGet("updates")]
        public Task<IEnumerable<string>> GetUpdates()
        {
            return _github.GetReleasesAsync(); 
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(string tag)
        {
            _appHost.StopApplication();
            //await _docker.UpdateImageAsync(tag);
            return Ok($"Ready to update version: { tag }");
        }
    }
}
