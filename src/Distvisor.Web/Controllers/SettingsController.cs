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
        private readonly IDockerService _docker;
        private readonly IGithubService _github;

        public SettingsController(IDockerService docker, IGithubService github)
        {
            _docker = docker;
            _github = github;
        }

        [HttpGet("updates")]
        public Task<IEnumerable<string>> GetUpdates()
        {
            return _github.GetReleasesAsync();
        }

        [HttpPost("update")]
        public IActionResult Update(string tag)
        {
            return Ok($"Redy to update version: { tag }");
        }
    }
}
