using Distvisor.App.Core.Dispatchers;
using Distvisor.App.Features.Backups.Commands.CreateBackup;
using Distvisor.App.Features.Backups.Commands.DeleteBackup;
using Distvisor.App.Features.Backups.Commands.RenameBackup;
using Distvisor.App.Features.Backups.Commands.RestoreBackup;
using Distvisor.App.Features.Backups.Queries.GetBackupFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/backups")]
    public class BackupsController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;        

        public BackupsController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<BackupFilesListDto> GetBackups([FromQuery] GetBackupFiles query, CancellationToken cancellationToken)
        {
            return await _dispatcher.DispatchAsync(query, cancellationToken);
        }

        [HttpPost]
        public async Task CreateBackup(CreateBackup command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }

        [HttpDelete]
        public async Task DeleteBackup(DeleteBackup command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }

        [HttpPatch]
        public async Task RenameBackup(RenameBackup command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }

        [HttpPost("restore")]
        public async Task RestoreBackup(RestoreBackup command, CancellationToken cancellationToken)
        {
            await _dispatcher.DispatchAsync(command, cancellationToken);
        }
    }
}