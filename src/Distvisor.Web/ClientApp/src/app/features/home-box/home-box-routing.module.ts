import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { DevicesComponent } from './devices/devices.component';
import { GatewaySessionsComponent } from './gateway-sessions/gateway-sessions.component';


const routes: Routes = [
  { path: 'home-box/devices', component: DevicesComponent, pathMatch: 'full', canActivate: [MsalGuard] },
  { path: 'home-box/gateway-sessions', component: GatewaySessionsComponent, pathMatch: 'full', canActivate: [MsalGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeBoxRoutingModule {}