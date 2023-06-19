import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms'

import { NgxsModule } from '@ngxs/store';

import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { MenuModule } from 'primeng/menu';
import { PanelModule } from 'primeng/panel';
import { ProgressBarModule } from 'primeng/progressbar';
import { TableModule } from 'primeng/table';

import { ApiModule } from 'src/app/api/api.module';
import { RedirectionsRoutingModule } from './redirections-routing.module';

import { RedirectionsState } from './store/redirections.state';

import { SharedModule } from 'src/app/shared';
import { RedirectionsComponent } from './redirections/redirections.component';
import { RedirectionsConfirmModalComponent } from './redirections/redirections-confirm-modal/redirections-confirm-modal.component';
import { RedirectionsInputModalComponent } from './redirections/redirections-input-modal/redirections-input-modal.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,

    // Ngxs
    NgxsModule.forFeature([RedirectionsState]),

    // PrimeNg
    ButtonModule,
    CardModule,
    DialogModule,
    InputTextModule,
    MenuModule,
    PanelModule,
    ProgressBarModule,
    TableModule,

    // internal
    ApiModule,
    SharedModule,
    RedirectionsRoutingModule
  ],
  declarations: [
    RedirectionsComponent,
    RedirectionsConfirmModalComponent,
    RedirectionsInputModalComponent,
  ]
})
export class RedirectionsModule { }
