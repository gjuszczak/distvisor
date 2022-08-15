import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { CardModule } from 'primeng/card';
import { CodeHighlighterModule } from 'primeng/codehighlighter';
import { TableModule } from 'primeng/table';

import { ApiModule } from 'src/app/api/api.module';
import { EventLogRoutingModule } from './event-log-routing.module';

import { EventLogComponent } from './event-log/event-log.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,

    // PrimeNg
    CardModule,
    CodeHighlighterModule,
    TableModule,

    // internal
    ApiModule,
    EventLogRoutingModule
  ],
  declarations: [EventLogComponent]
})
export class EventLogModule { }
