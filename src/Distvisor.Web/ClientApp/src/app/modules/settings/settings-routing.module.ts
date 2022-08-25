import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { SettingsComponent } from './settings/settings.component';

const routes: Routes = [
    { path: 'settings', component: SettingsComponent, pathMatch: 'full', canActivate: [MsalGuard] },  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingsRoutingModule {}