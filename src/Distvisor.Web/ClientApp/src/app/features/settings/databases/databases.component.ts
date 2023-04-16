import { Component, OnInit } from '@angular/core';
import { AdminDeprService } from 'src/app/api/services';
import { MenuItem } from 'primeng/api';
import { BackupFileInfoDto } from 'src/app/api/models';


@Component({
  selector: 'app-databases',
  templateUrl: './databases.component.html'
})
export class DatabasesComponent implements OnInit {

  dbBtnMenuItems: MenuItem[] = [];
  bakBtnMenuItems: {[key:string]: MenuItem[]} = {};
  backupFiles: BackupFileInfoDto[] = [];

  constructor(private adminService: AdminDeprService) { }

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
    this.adminService.apiSecAdminListBackupsGet$Json()
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
    this.adminService.apiSecAdminCreateBackupPost()
      .subscribe(() => this.reloadBackupList());
  }

  replayEvents() {
    this.adminService.apiSecAdminReplayEventsPost()
      .subscribe(() => location.reload());
  }

  restore(backupFile: BackupFileInfoDto) {
    this.adminService.apiSecAdminRestoreBackupPost({
      body: backupFile
    }).subscribe(() => this.reloadBackupList());
  }

  remove(backupFile: BackupFileInfoDto) {
    this.adminService.apiSecAdminDeleteBackupPost({
      body: backupFile
    }).subscribe(() => this.reloadBackupList());
  }
}
