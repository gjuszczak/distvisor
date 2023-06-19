import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { BackupsComponent } from './backups/backups.component';

const routes: Routes = [
  { path: 'admin/backups', component: BackupsComponent, pathMatch: 'full', canActivate: [MsalGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BackupsRoutingModule {}