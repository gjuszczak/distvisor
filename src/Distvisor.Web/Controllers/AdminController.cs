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
        private readonly IDeploymentService _deploymentService;
        private readonly IBackupService _backupService;

        public AdminController(IDeploymentService deploymentService, IBackupService backupService)
        {
            _deploymentService = deploymentService;
            _backupService = backupService;
        }

        [HttpGet("deployment-params")]
        public async Task<DeploymentParamsResponseDto> GetDeploymentParams()
        {
            var versions = await _deploymentService.GetReleasesAsync();
            var environments = _deploymentService.Environments;
            return new DeploymentParamsResponseDto
            {
                Versions = versions,
                Environments = environments,
            };
        }

        [HttpPost("deploy")]
        public Task Deploy([FromBody]DeployRequestDto dto)
        {
            return _deploymentService.DeployVersionAsync(dto.Environment, dto.Version);
        }

        [HttpPost("redeploy")]
        public Task Redeploy([FromBody]RedeployRequestDto dto)
        {
            return _deploymentService.RedeployAsync(dto.Environment);
        }


        [HttpGet("list-backups")]
        public async Task<List<BackupFileInfoDto>> ListBackups()
        {
            var files = (await _backupService.ListStoredBackupsAsync())
                .Select(x => new BackupFileInfoDto
                {
                    Name = x.Name,
                    CreatedDateTime = x.CreatedDateTime,
                    Size = x.Size
                })
                .ToList();

            return files;
        }

        [HttpPost("create-backup")]
        public async Task CreateBackup()
        {
            await _backupService.CreateBackupAsync();
        }

        [HttpPost("restore-backup")]
        public async Task RestoreBackup([FromBody]BackupFileInfoDto dto)
        {
            await _backupService.RestoreBackupAsync(dto.Name);
        }

        [HttpPost("delete-backup")]
        public async Task DeleteBackup([FromBody]BackupFileInfoDto dto)
        {
            await _backupService.DeleteBackupAsync(dto.Name);
        }

        [HttpPost("replay-events")]
        public async Task ReplayEvents()
        {
            await _backupService.ReplayEventsToReadStoreAsync();
        }
    }

    public class DeploymentParamsResponseDto
    {
        public IEnumerable<string> Versions { get; set; }
        public IEnumerable<string> Environments { get; set; }
    }

    public class BackupFileInfoDto
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

    public class DeployRequestDto
    {
        public string Environment { get; set; }
        public string Version { get; set; }
    }

    public class RedeployRequestDto
    {
        public string Environment { get; set; }
    }
}