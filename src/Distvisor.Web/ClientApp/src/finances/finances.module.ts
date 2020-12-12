import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ChipsModule } from 'primeng/chips';
import { DialogModule } from 'primeng/dialog';
import { FileUploadModule } from 'primeng/fileupload';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { RadioButtonModule } from 'primeng/radiobutton';
import { RippleModule } from 'primeng/ripple';
import { TableModule } from 'primeng/table';

import { AddAccountDialogComponent } from './add-acccount-dialog/add-account-dialog.component';
import { FinancesComponent } from './finances/finances.component';
import { ImportFinancialFilesDialogComponent } from './import-financial-files-dialog/import-financial-files-dialog.component';

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
    ChipsModule,
    DialogModule,
    FileUploadModule,
    InputNumberModule,
    InputTextModule,
    RadioButtonModule,
    RippleModule,
    TableModule
  ],
  declarations: [
    AddAccountDialogComponent,
    FinancesComponent,
    ImportFinancialFilesDialogComponent
  ]
})
export class FinancesModule { }
