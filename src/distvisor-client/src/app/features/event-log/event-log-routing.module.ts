import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { EventLogComponent } from './event-log/event-log.component';

const routes: Routes = [
  { path: 'admin/event-log', component: EventLogComponent, pathMatch: 'full' },
  { path: 'admin/event-log/aggregates/:aggregateId', component: EventLogComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EventLogRoutingModule {}
