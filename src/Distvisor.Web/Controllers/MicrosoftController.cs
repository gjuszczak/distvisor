using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MicrosoftController : ControllerBase
    {
        private readonly IMicrosoftOneDriveService _oneDriveService;

        public MicrosoftController(IMicrosoftOneDriveService oneDriveService)
        {
            _oneDriveService = oneDriveService;
        }

        [Authorize]
        [HttpGet("backup")]
        public async Task Backup()
        {
            await _oneDriveService.BackupDb();
        }
    }

    public class MicrosoftAuthDto
    {
        public string AuthUri { get; set; }
    }
}
