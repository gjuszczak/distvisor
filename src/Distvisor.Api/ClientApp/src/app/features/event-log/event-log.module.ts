import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { NgxsModule } from '@ngxs/store';

import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DialogModule } from 'primeng/dialog';
import { CodeHighlighterModule } from 'primeng/codehighlighter';
import { InputTextModule } from 'primeng/inputtext';
import { MenuModule } from 'primeng/menu';
import { PanelModule } from 'primeng/panel';
import { ProgressBarModule } from 'primeng/progressbar';
import { TableModule } from 'primeng/table';

import { ApiModule } from 'src/app/api/api.module';
import { EventLogRoutingModule } from './event-log-routing.module';

import { EventLogState } from './store/event-log.state';
import { EventsState } from './store/events.state';
import { AggregateState } from './store/aggregate.state';

import { EventLogComponent } from './event-log/event-log.component';
import { EventLogConfirmModalComponent } from './event-log/event-log-confirm-modal/event-log-confirm-modal.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,

    // Ngxs
    NgxsModule.forFeature([EventLogState, EventsState, AggregateState]),

    // PrimeNg
    ButtonModule,
    CardModule,
    DialogModule,
    CodeHighlighterModule,
    InputTextModule,
    MenuModule,
    PanelModule,
    ProgressBarModule,
    TableModule,

    // internal
    ApiModule,
    EventLogRoutingModule
  ],
  declarations: [EventLogComponent, EventLogConfirmModalComponent]
})
export class EventLogModule { }
