import { createSelector } from '@ngrx/store';
import { EventLogState } from './event-log.state';

export const selectEventLog = (state: EventLogState) => state.eventLog;

export const selectEventLogEntries = createSelector(
  selectEventLog,
  eventLog => eventLog.entries
);