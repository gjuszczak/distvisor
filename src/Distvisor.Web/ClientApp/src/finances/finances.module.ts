import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { CardModule } from 'primeng/card';
import { FileUploadModule } from 'primeng/fileupload';

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
    CardModule,
    FileUploadModule,
  ],
  declarations: [FinancesComponent]
})
export class FinancesModule { }
