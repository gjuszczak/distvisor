using Distvisor.App.Common.Models;
using System.Collections.Generic;

namespace Distvisor.App.Admin.Qureies.GetBackupFiles
{
    public class BackupFilesListDto : PaginatedList<BackupFileDto>
    {
        public BackupFilesListDto(List<BackupFileDto> items, int totalRecords, int first, int rows)
            : base(items, totalRecords, first, rows) { }
    }
}
