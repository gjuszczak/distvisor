import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { CodeHighlighterModule } from 'primeng/codehighlighter';
import { DialogModule } from 'primeng/dialog';
import { DataViewModule } from 'primeng/dataview';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { RippleModule } from 'primeng/ripple';
import { TableModule } from 'primeng/table';

import { ApiModule } from '../api/api.module';
import { HomeBoxComponent } from './home-box/home-box.component';
import { TriggersComponent } from './triggers/triggers.component';
import { DevicesListComponent } from './devices-list/devices-list.component';
import { DeviceDetailsDialogComponent } from './device-details-dialog/device-details-dialog.component';

@NgModule({
  imports: [
    ButtonModule,
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'home-box', component: HomeBoxComponent, pathMatch: 'full', canActivate: [MsalGuard] },
    ]),

    // PrimeNg
    ButtonModule,
    CardModule,
    CodeHighlighterModule,
    DialogModule,
    DataViewModule,
    InputTextareaModule,
    InputTextModule,
    RippleModule,
    TableModule,

    // internal
    ApiModule,
  ],
  declarations: [
    HomeBoxComponent,
    DevicesListComponent,
    TriggersComponent,
    DeviceDetailsDialogComponent,
  ]
})
export class HomeBoxModule { }
