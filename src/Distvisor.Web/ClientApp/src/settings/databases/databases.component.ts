import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/api/services';
import { MenuItem } from 'primeng/api';
import { BackupFileInfoDto } from 'src/api/models';


@Component({
  selector: 'app-databases',
  templateUrl: './databases.component.html'
})
export class DatabasesComponent implements OnInit {

  dbBtnMenuItems: MenuItem[] = [];
  bakBtnMenuItems: {[key:string]: MenuItem[]} = {};
  backupFiles: BackupFileInfoDto[] = [];

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.dbBtnMenuItems = [
      {
        label: 'Replay events to RS',
        icon: 'pi pi-refresh',
        command: () => {
          this.replayEvents();
        }
      }
    ];

    this.bakBtnMenuItems = {};
    this.reloadBackupList();
  }

  reloadBackupList() {
    this.adminService.apiAdminListBackupsGet$Json()
      .subscribe(backups => {
        this.backupFiles = backups;
        
        backups.forEach(b => {
          this.bakBtnMenuItems[b.name || ''] = [
            {
              label: 'Remove',
              icon: 'pi pi-times',
              command: () => {
                this.remove(b);
              }
            }
          ]
        })
      });
  }

  isLast(backupFile: BackupFileInfoDto) {
    return this.backupFiles.indexOf(backupFile) === this.backupFiles.length - 1;
  }

  create() {
    this.adminService.apiAdminCreateBackupPost()
      .subscribe(() => this.reloadBackupList());
  }

  replayEvents() {
    this.adminService.apiAdminReplayEventsPost()
      .subscribe(() => location.reload());
  }

  restore(backupFile: BackupFileInfoDto) {
    this.adminService.apiAdminRestoreBackupPost({
      body: backupFile
    }).subscribe(() => this.reloadBackupList());
  }

  remove(backupFile: BackupFileInfoDto) {
    this.adminService.apiAdminDeleteBackupPost({
      body: backupFile
    }).subscribe(() => this.reloadBackupList());
  }
}
