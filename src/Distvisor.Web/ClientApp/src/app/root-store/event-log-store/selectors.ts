import { createFeatureSelector, createSelector } from '@ngrx/store';
import { State } from './state';

export const selectEventLog = createFeatureSelector<State>('eventLog');

export const selectEventLogEntries = createSelector(
  selectEventLog,
  eventLog => eventLog.entries
);