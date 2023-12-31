/* tslint:disable */
/* eslint-disable */
import { EventDto } from './event-dto';
export interface EventsListDto {
  aggregateId?: null | string;
  first?: number;
  items?: null | Array<EventDto>;
  rows?: number;
  totalRecords?: number;
}
