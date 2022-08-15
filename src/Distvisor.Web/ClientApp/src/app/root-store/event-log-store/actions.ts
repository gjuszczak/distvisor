import { createAction, props } from '@ngrx/store';
import { EventsLogEntryDtoPaginatedList } from 'src/app/api/models';

export const loadEventLogEntries = createAction(
  '[Event Log] Load Event Log Entries',
  props<{ firstOffset?: number, pageSize?: number }>()
);

export const eventLogEntriesLoadedSuccess = createAction(
  '[Event Log] Event Log Entries Loaded Success',
  props<{ eventLogEntries: EventsLogEntryDtoPaginatedList }>()
);
