import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FinancesRoutingModule } from './finances-routing.module';
import { FinancesComponent } from './finances/finances.component';

@NgModule({
  imports: [
    CommonModule,

    // internal
    FinancesRoutingModule
  ],
  declarations: [
    FinancesComponent,
  ]
})
export class FinancesModule { }
