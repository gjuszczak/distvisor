import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DevicesComponent } from './devices/devices.component';
import { GatewaySessionsComponent } from './gateway-sessions/gateway-sessions.component';


const routes: Routes = [
  { path: 'home-box/devices', component: DevicesComponent, pathMatch: 'full' },
  { path: 'home-box/gateway-sessions', component: GatewaySessionsComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeBoxRoutingModule {}
