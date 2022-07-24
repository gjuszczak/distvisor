/* tslint:disable */
/* eslint-disable */
export interface EventsLogEntryDto {
  aggregateId?: string;
  aggregateType?: null | string;
  aggregateTypeDisplayName?: null | string;
  correlationId?: string;
  eventId?: string;
  eventType?: null | string;
  eventTypeDisplayName?: null | string;
  maskedPayload?: null | any;
  timeStamp?: string;
  version?: number;
}
