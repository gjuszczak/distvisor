import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { DataViewModule } from 'primeng/dataview';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { PanelModule } from 'primeng/panel';

import { InvoicesComponent } from './invoices/invoices.component';
import { AuthGuard } from '../auth/auth.guard';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'invoices', component: InvoicesComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    ]),

    // PrimeNg
    DataViewModule,
    DialogModule,
    DropdownModule,
    PanelModule,
  ],
  declarations: [InvoicesComponent]
})
export class InvoicesModule { }
