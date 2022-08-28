import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { NgxsModule } from '@ngxs/store';

import { CardModule } from 'primeng/card';
import { CodeHighlighterModule } from 'primeng/codehighlighter';
import { TableModule } from 'primeng/table';

import { ApiModule } from 'src/app/api/api.module';
import { EventLogRoutingModule } from './event-log-routing.module';

import { EventLogState } from './store/event-log.state';
import { EventLogComponent } from './event-log/event-log.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,

    // Ngxs
    NgxsModule.forFeature([EventLogState]),

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
