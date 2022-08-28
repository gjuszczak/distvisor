import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { FinancesAccountComponent } from './finances-account/finances-account.component';
import { FinancesComponent } from './finances/finances.component';

const routes: Routes = [
  { path: 'finances', component: FinancesComponent, canActivate: [MsalGuard] },
  { path: 'finances/account/:id', component: FinancesAccountComponent, canActivate: [MsalGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FinancesRoutingModule { }