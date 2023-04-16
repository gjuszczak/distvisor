using Distvisor.App.Admin.Services;
using Distvisor.App.Common.Models;
using Distvisor.App.Core.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Admin.Qureies.GetBackupFiles
{
    public class GetBackupFiles : PaginatedQuery, IQuery<BackupFilesListDto>
    {
    }

    public class GetBackupFilesHandler : IQueryHandler<GetBackupFiles, BackupFilesListDto>
    {
        private readonly IBackupService _backupService;

        public GetBackupFilesHandler(IBackupService backupService)
        {
            _backupService = backupService;
        }

        public async Task<BackupFilesListDto> Handle(GetBackupFiles request, CancellationToken cancellationToken)
        {
            return await _backupService.GetAsync(request.First, request.Rows, cancellationToken);
        }
    }
}
