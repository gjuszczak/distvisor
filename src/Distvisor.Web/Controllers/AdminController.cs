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
    public class AdminController : ControllerBase
    {
        private readonly IMicrosoftOneDriveService _oneDriveService;
        private readonly IDeploymentService _deploymentService;

        public AdminController(IDeploymentService deploymentService, IMicrosoftOneDriveService oneDriveService)
        {
            _deploymentService = deploymentService;
            _oneDriveService = oneDriveService;
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


        [HttpPost("backup")]
        public async Task Backup()
        {
            await _oneDriveService.BackupDb();
        }
    }

    public class DeploymentParamsResponseDto
    {
        public IEnumerable<string> Versions { get; set; }
        public IEnumerable<string> Environments { get; set; }
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