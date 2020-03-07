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
        private readonly INotificationService _notifications;

        public SettingsController(IGithubService github, INotificationService notifications)
        {
            _github = github;
            _notifications = notifications;
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
        public async Task Update([FromBody]UpdateRequestDto dto)
        {
            await _github.UpdateToVersionAsync(dto.UpdateToVersion, dto.DbUpdateStrategy.ToString());
            await _notifications.PushSuccessAsync($"Update to { dto.UpdateToVersion } started.");
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