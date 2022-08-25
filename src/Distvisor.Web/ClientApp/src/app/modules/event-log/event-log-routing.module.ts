import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { EventLogComponent } from './event-log/event-log.component';

const routes: Routes = [
  { path: 'event-log', component: EventLogComponent, pathMatch: 'full', canActivate: [MsalGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EventLogRoutingModule {}