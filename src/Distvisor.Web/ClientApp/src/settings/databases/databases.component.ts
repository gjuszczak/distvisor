import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/api/services';
import { MenuItem } from 'primeng/api';
import { BackupFileInfoDto } from 'src/api/models';


@Component({
  selector: 'app-databases',
  templateUrl: './databases.component.html'
})
export class DatabasesComponent implements OnInit {

  splitBtnBackupItems: MenuItem[];
  esBackups: BackupFileInfoDto[];

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.splitBtnBackupItems = [
      {
        label: 'Recreate',
        icon: 'pi pi-fw pi-refresh',
        command: () => {
          this.save();
        }
      }
    ];

    this.adminService.apiAdminListBackupsGet$Json()
      .subscribe(backups => {
        this.esBackups = backups;
      });
  }

  isLast(backup: BackupFileInfoDto) {
    return this.esBackups.indexOf(backup) === this.esBackups.length -1;
  }

  save() {

  }
}
