import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/api/services';
import { MenuItem } from 'primeng/api';
import { BackupFileInfoDto } from 'src/api/models';
import { Menu } from 'primeng/menu';


@Component({
  selector: 'app-databases',
  templateUrl: './databases.component.html'
})
export class DatabasesComponent implements OnInit {

  btnMenuItems: MenuItem[];
  backupFiles: BackupFileInfoDto[];

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.btnMenuItems = [
      {
        label: 'Replay events to RS',
        icon: 'pi pi-refresh',
        command: () => {
          this.save();
        }
      }
    ];

    this.adminService.apiAdminListBackupsGet$Json()
      .subscribe(backups => {
        this.backupFiles = backups;
      });
  }

  isLast(backup: BackupFileInfoDto) {
    return this.backupFiles.indexOf(backup) === this.backupFiles.length - 1;
  }

  save() {
    this.adminService.apiAdminBackupPost().subscribe();
  }
}
