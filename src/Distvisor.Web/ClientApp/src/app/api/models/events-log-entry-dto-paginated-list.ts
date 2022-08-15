/* tslint:disable */
/* eslint-disable */
import { EventsLogEntryDto } from './events-log-entry-dto';
export interface EventsLogEntryDtoPaginatedList {
  firstOffset?: number;
  items?: null | Array<EventsLogEntryDto>;
  pageSize?: number;
  totalCount?: number;
}
