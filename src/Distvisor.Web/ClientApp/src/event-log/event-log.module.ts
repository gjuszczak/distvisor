import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';

import { EventLogComponent } from './event-log/event-log.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'event-log', component: EventLogComponent, pathMatch: 'full', canActivate: [MsalGuard] },
    ]),

    // PrimeNg
    ButtonModule,
    TableModule,
  ],
  declarations: [EventLogComponent]
})
export class EventLogModule { }
