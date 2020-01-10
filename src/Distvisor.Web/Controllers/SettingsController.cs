using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IGithubService _github;

        public SettingsController(IGithubService github)
        {
            _github = github;
        }

        [HttpGet("update-params")]
        public async Task<UpdateParamsResponseDto> GetUpdateParams()
        {
            var versions = await _github.GetReleasesAsync();
            var strategies = Enum.GetValues(typeof(DbUpdateStrategy)).Cast<DbUpdateStrategy>();
            return new UpdateParamsResponseDto
            {
                Versions = versions,
                DbUpdateStrategies = strategies,
            };
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody]UpdateRequestDto dto)
        {
            await _github.UpdateToVersion(dto.UpdateToVersion, dto.DbUpdateStrategy.ToString());
            return Ok($"Update to { dto.UpdateToVersion } started.");
        }
    }

    public class UpdateParamsResponseDto
    {
        public IEnumerable<string> Versions { get; set; }
        public IEnumerable<DbUpdateStrategy> DbUpdateStrategies { get; set; }
    }

    public class UpdateRequestDto { 
        public string UpdateToVersion { get; set; }

        public DbUpdateStrategy DbUpdateStrategy { get; set; }
    }

    public enum DbUpdateStrategy {
        MigrateToLatest,
        EmptyDatabase,
    }
}