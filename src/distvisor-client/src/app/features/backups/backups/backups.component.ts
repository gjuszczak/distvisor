import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Store, Select } from '@ngxs/store';
import { Observable } from 'rxjs';
import { MenuItem } from 'primeng/api';
import { TableLazyLoadEvent } from 'primeng/table';

import { BackupFileDto } from 'src/app/api/models';
import { BackupModal, BackupModals, BackupsList, BackupsState } from '../store/backups.state';
import { CreateBackup, DeleteBackup, HideBackupModal, LoadBackups, RenameBackup, RestoreBackup, ShowBackupModal } from '../store/backups.actions';

@Component({
  selector: 'app-backups',
  templateUrl: './backups.component.html',
  providers: [DatePipe]
})
export class BackupsComponent implements OnInit {

  @Select(BackupsState.getBackups)
  public readonly backups$!: Observable<BackupsList>;

  @Select(BackupsState.getModal)
  public readonly modal$!: Observable<BackupModal>;

  public readonly BackupModals = BackupModals;

  public backupMenuItems: MenuItem[] = [];
  public selectedBackupName: string = '';
  private firstLazyLoad: boolean = true;

  constructor(
    private readonly store: Store,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly datePipe: DatePipe
  ) { }

  ngOnInit(): void {
    this.backupMenuItems = [
      {
        label: 'Rename',
        icon: 'pi pi-pencil',
        command: () => {
          this.store.dispatch(new ShowBackupModal(BackupModals.Rename, this.selectedBackupName));
        }
      },
      {
        label: 'Restore',
        icon: 'pi pi-cloud-download',
        command: () => {
          this.store.dispatch(new ShowBackupModal(BackupModals.ConfirmRestore));
        }
      },
      {
        label: 'Delete',
        icon: 'pi pi-trash',
        command: () => {
          this.store.dispatch(new ShowBackupModal(BackupModals.ConfirmDelete));
        }
      }
    ];

    this.route.queryParams.subscribe(({ first, rows }) => {
      this.reloadBackups(first, rows);
    });
  }

  toggleBackupMenu(menu: any, event: any, backup: BackupFileDto) {
    if (backup?.name) {
      this.selectedBackupName = backup.name;
      menu.toggle(event);
    }
  }

  showCreateBackupModal() {
    const defaultInputText = `events_db_${this.datePipe.transform(new Date(), 'yyyyMMdd_HHmmss')}.bak`;
    this.store.dispatch(new ShowBackupModal(BackupModals.Create, defaultInputText));
  }

  lazyLoadBackups({ first, rows }: TableLazyLoadEvent) {
    if (this.firstLazyLoad) {
      this.firstLazyLoad = false;
      return;
    }

    this.router.navigate([], { queryParams: { first, rows } });
  }

  reloadBackups(first?: number, rows?: number) {
    this.store.dispatch(new LoadBackups(first, rows));
  }

  hideBackupModal() {
    this.store.dispatch(new HideBackupModal());
  }

  createBackup(inputText: string) {
    this.store.dispatch(new CreateBackup(inputText));
  }

  renameBackup(inputText: string) {
    this.store.dispatch(new RenameBackup(this.selectedBackupName, inputText));
  }

  deleteBackup() {
    this.store.dispatch(new DeleteBackup(this.selectedBackupName));
  }

  restoreBackup() {
    this.store.dispatch(new RestoreBackup(this.selectedBackupName));
  }
}
