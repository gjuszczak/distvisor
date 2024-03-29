﻿using Distvisor.App.Core.Commands;
using Distvisor.App.Features.Backups.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.Backups.Commands.DeleteBackup
{
    public class DeleteBackup : Command
    {
        public string Name { get; set; }
    }

    public class DeleteBackupHandler : ICommandHandler<DeleteBackup>
    {
        private readonly IBackupService _backupService;

        public DeleteBackupHandler(IBackupService backupService)
        {
            _backupService = backupService;
        }

        public async Task<Guid> Handle(DeleteBackup command, CancellationToken cancellationToken)
        {
            await _backupService.DeleteAsync(command.Name, cancellationToken);
            return command.CorrelationId;
        }
    }
}
