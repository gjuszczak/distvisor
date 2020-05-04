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
    public class AdminController : ControllerBase
    {
        private readonly IMicrosoftOneDriveService _oneDriveService;
        private readonly IUpdateService _updateService;

        public AdminController(IUpdateService updateService, IMicrosoftOneDriveService oneDriveService)
        {
            _updateService = updateService;
            _oneDriveService = oneDriveService;
        }

        [HttpGet("update-params")]
        public async Task<UpdateParamsResponseDto> GetUpdateParams()
        {
            var versions = await _updateService.GetReleasesAsync();
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
            await _updateService.UpdateToVersionAsync(dto.UpdateToVersion, dto.DbUpdateStrategy.ToString());
        }


        [HttpPost("backup")]
        public async Task Backup()
        {
            await _oneDriveService.BackupDb();
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