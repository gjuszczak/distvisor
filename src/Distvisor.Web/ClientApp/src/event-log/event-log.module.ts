import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { TableModule } from 'primeng/table';
import { CodeHighlighterModule } from 'primeng/codehighlighter';

import { EventLogComponent } from './event-log/event-log.component';
import { ApiModule } from '../api/api.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'event-log', component: EventLogComponent, pathMatch: 'full', canActivate: [MsalGuard] },
    ]),

    // PrimeNg
    TableModule,
    CodeHighlighterModule,

    // internal
    ApiModule,
  ],
  declarations: [EventLogComponent]
})
export class EventLogModule { }
