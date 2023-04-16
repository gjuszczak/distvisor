using Distvisor.App.Admin.Services;
using Distvisor.App.Common.Interfaces;
using Distvisor.App.Core.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Admin.Commands.RestoreBackup
{
    public class RestoreBackup : Command
    {
        public string Name { get; set; }
    }

    public class RestoreBackupHandler : ICommandHandler<RestoreBackup>
    {
        private readonly IBackupService _backupService;
        private readonly IEventsReplayService _eventsReplayService;

        public RestoreBackupHandler(IBackupService backupService, IEventsReplayService eventsReplayService)
        {
            _backupService = backupService;
            _eventsReplayService = eventsReplayService;
        }

        public async Task<Guid> Handle(RestoreBackup command, CancellationToken cancellationToken)
        {
            await _backupService.RestoreAsync(command.Name, cancellationToken);
            await _eventsReplayService.ReplayAsync(cancellationToken);
            return command.CorrelationId;            
        }
    }
}
