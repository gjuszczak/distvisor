import { createReducer, on } from '@ngrx/store';
import { EventsLogEntryDto } from 'src/api/models/events-log-entry-dto';
import { PaginatedList } from 'src/shared';

export const initialState: PaginatedList<EventsLogEntryDto> = {
  items: [],
  firstOffset: 0,
  pageSize: 10,
  pageSizeOptions: [10, 25, 50, 100],
  totalCount: 0,
  loading: false
};

export const eventsReducer = createReducer(
  initialState,
);