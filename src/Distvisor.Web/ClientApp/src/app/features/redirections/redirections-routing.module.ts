import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { RedirectionsComponent } from './redirections/redirections.component';

const routes: Routes = [
  { path: 'admin/redirections', component: RedirectionsComponent, pathMatch: 'full', canActivate: [MsalGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RedirectionsRoutingModule {}