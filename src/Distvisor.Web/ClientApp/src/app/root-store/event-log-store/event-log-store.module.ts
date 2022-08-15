import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { EventLogEffects } from './effects';
import { entriesReducer } from './reducer';

@NgModule({
  imports: [
    CommonModule,
    StoreModule.forFeature('eventLog', {
      entries: entriesReducer,
    }),
    EffectsModule.forFeature([
      EventLogEffects,
    ]),
  ],
  providers: [EventLogEffects]
})
export class EventLogStoreModule {}