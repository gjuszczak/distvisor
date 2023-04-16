using System;

namespace Distvisor.App.Admin.Qureies.GetBackupFiles
{
    public class BackupFileDto
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
    }
}
