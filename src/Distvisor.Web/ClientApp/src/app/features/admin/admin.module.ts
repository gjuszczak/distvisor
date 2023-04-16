import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { NgxsModule } from '@ngxs/store';

import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { MenuModule } from 'primeng/menu';
import { PanelModule } from 'primeng/panel';
import { ProgressBarModule } from 'primeng/progressbar';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { TableModule } from 'primeng/table';

import { ApiModule } from 'src/app/api/api.module';
import { AdminRoutingModule } from './admin-routing.module';

import { AdminState } from './store/admin.state';
import { BackupsState } from './store/backups.state';

import { SharedModule } from 'src/app/shared';
import { AdminComponent } from './admin/admin.component';
import { BackupsComponent } from './backups/backups.component';
import { BackupConfirmModalComponent } from './backups/backup-confirm-modal/backup-confirm-modal.component';
import { BackupInputModalComponent } from './backups/backup-input-modal/backup-input-modal.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,

    // Ngxs
    NgxsModule.forFeature([AdminState, BackupsState]),

    // PrimeNg
    ButtonModule,
    CardModule,
    DialogModule,
    InputTextModule,
    MenuModule,
    PanelModule,
    ProgressBarModule,
    ProgressSpinnerModule,
    TableModule,

    // internal
    ApiModule,
    SharedModule,
    AdminRoutingModule
  ],
  declarations: [
    AdminComponent, 
    BackupsComponent,
    BackupConfirmModalComponent,
    BackupInputModalComponent,
  ]
})
export class AdminModule { }
