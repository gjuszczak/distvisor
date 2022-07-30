import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { entriesReducer } from './state/entries.reducer';
import { EntriesEffects } from './state/entries.effects';

import { CardModule } from 'primeng/card';
import { CodeHighlighterModule } from 'primeng/codehighlighter';
import { TableModule } from 'primeng/table';

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

    // NgRx
    StoreModule.forFeature('eventLog', {
      entries: entriesReducer,
    }),
    EffectsModule.forFeature([
      EntriesEffects,
    ]),

    // PrimeNg
    CardModule,
    CodeHighlighterModule,
    TableModule,

    // internal
    ApiModule,
  ],
  declarations: [EventLogComponent]
})
export class EventLogModule { }
