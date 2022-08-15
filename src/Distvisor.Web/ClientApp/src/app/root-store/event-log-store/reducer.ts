import { createReducer, on } from '@ngrx/store';
import * as EventLogActions from './actions';
import { EventsLogEntryDto } from 'src/app/api/models/events-log-entry-dto';
import { PaginatedList } from 'src/app/shared';

export const initialState: PaginatedList<EventsLogEntryDto> = {
  items: [],
  firstOffset: 0,
  pageSize: 10,
  pageSizeOptions: [10, 25, 50, 100],
  totalCount: 0,
  loading: false
};

export const entriesReducer = createReducer(
  initialState,
  on(EventLogActions.loadEventLogEntries, (state) => ({
    ...state,
    loading: true
  })),
  on(EventLogActions.eventLogEntriesLoadedSuccess, (_, { eventLogEntries }) => ({
    items: eventLogEntries.items ?? initialState.items,
    firstOffset: eventLogEntries.firstOffset ?? initialState.firstOffset,
    pageSize: eventLogEntries.pageSize ?? initialState.pageSize,
    totalCount: eventLogEntries.totalCount ?? initialState.totalCount,
    pageSizeOptions: initialState.pageSizeOptions,
    loading: false
  })),
);