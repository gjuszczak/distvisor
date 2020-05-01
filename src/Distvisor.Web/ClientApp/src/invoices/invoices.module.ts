import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { DataViewModule } from 'primeng/dataview';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { FieldsetModule } from 'primeng/fieldset';
import { SpinnerModule } from 'primeng/spinner';
import { PanelModule } from 'primeng/panel';

import { InvoicesComponent } from './invoices/invoices.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'invoices', component: InvoicesComponent, pathMatch: 'full', canActivate: [MsalGuard] },
    ]),

    // PrimeNg
    ButtonModule,
    CalendarModule,
    DataViewModule,
    DialogModule,
    DropdownModule,
    FieldsetModule,
    PanelModule,
    SpinnerModule,
  ],
  declarations: [InvoicesComponent]
})
export class InvoicesModule { }
