using Distvisor.App.Admin.Services;
using Distvisor.App.Core.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Admin.Commands.RenameBackup
{
    public class RenameBackup : Command
    {
        public string OldName { get; set; }
        public string NewName { get; set; }
    }

    public class RenameBackupHandler : ICommandHandler<RenameBackup>
    {
        private readonly IBackupService _backupService;

        public RenameBackupHandler(IBackupService backupService)
        {
            _backupService = backupService;
        }

        public async Task<Guid> Handle(RenameBackup command, CancellationToken cancellationToken)
        {
            await _backupService.RenameAsync(command.OldName, command.NewName, cancellationToken);
            return command.CorrelationId;
        }
    }
}
