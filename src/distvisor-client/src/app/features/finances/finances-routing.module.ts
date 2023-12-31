import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { FinancesComponent } from './finances/finances.component';

const routes: Routes = [
  { path: 'finances', component: FinancesComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinancesRoutingModule { }
