import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { InvoicesComponent } from './invoices/invoices.component';
import { AuthGuard } from '../auth/auth.guard';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot([
      { path: 'invoices', component: InvoicesComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    ]),
  ],
  declarations: [InvoicesComponent]
})
export class InvoicesModule { }
