import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { CardModule } from 'primeng/card';
import { DataViewModule } from 'primeng/dataview';
import { DropdownModule } from 'primeng/dropdown';
import { InputNumberModule } from 'primeng/inputnumber';
import { RippleModule } from 'primeng/ripple';

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
    CardModule,
    DataViewModule,
    DropdownModule,
    InputNumberModule,
    RippleModule,
  ],
  declarations: [InvoicesComponent]
})
export class InvoicesModule { }
