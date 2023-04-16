using Distvisor.App.Admin.Commands.CreateBackup;
using Distvisor.App.Admin.Commands.DeleteBackup;
using Distvisor.App.Admin.Commands.RenameBackup;
using Distvisor.App.Admin.Commands.RestoreBackup;
using Distvisor.App.Admin.Qureies.GetBackupFiles;
using Distvisor.App.Core.Dispatchers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/s/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;        

        public AdminController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet("backups")]
        public async Task<BackupFilesListDto> GetBackups([FromQuery] GetBackupFiles query, CancellationToken cancellationToken)
        {
            return await _dispatcher.DispatchAsync(query, cancellationToken);
        }

        [HttpPost("backups")]
        public async Task CreateBackup(CreateBackup command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }

        [HttpDelete("backups")]
        public async Task DeleteBackup(DeleteBackup command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }

        [HttpPatch("backups")]
        public async Task RenameBackup(RenameBackup command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }

        [HttpPost("backups/restore")]
        public async Task RestoreBackup(RestoreBackup command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }
    }
}