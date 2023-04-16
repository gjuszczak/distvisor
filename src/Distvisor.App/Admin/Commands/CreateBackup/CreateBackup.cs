using Distvisor.App.Admin.Services;
using Distvisor.App.Core.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Admin.Commands.CreateBackup
{
    public class CreateBackup : Command
    {
        public string Name { get; set; }
    }

    public class CreateBackupHandler : ICommandHandler<CreateBackup>
    {
        private readonly IBackupService _backupService;

        public CreateBackupHandler(IBackupService backupService)
        {
            _backupService = backupService;
        }

        public async Task<Guid> Handle(CreateBackup command, CancellationToken cancellationToken)
        {
            await _backupService.CreateAsync(command.Name, cancellationToken);
            return command.CorrelationId;            
        }
    }
}
