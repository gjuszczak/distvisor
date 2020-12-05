import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DialogModule } from 'primeng/dialog';
import { FileUploadModule } from 'primeng/fileupload';
import { RippleModule } from 'primeng/ripple';
import { TableModule } from 'primeng/table';

import { AddAccountDialogComponent } from './add-acccount-dialog/add-account-dialog.component';
import { FinancesComponent } from './finances/finances.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'finances', component: FinancesComponent, pathMatch: 'full', canActivate: [MsalGuard] },
    ]),

    // PrimeNg
    ButtonModule,
    CardModule,
    DialogModule,
    FileUploadModule,
    RippleModule,
    TableModule
  ],
  declarations: [
    AddAccountDialogComponent,
    FinancesComponent
  ]
})
export class FinancesModule { }
