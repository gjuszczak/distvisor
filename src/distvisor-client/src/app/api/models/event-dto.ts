/* tslint:disable */
/* eslint-disable */
export interface EventDto {
  aggregateId?: string;
  aggregateType?: null | string;
  aggregateTypeDisplayName?: null | string;
  correlationId?: string;
  eventId?: number;
  eventType?: null | string;
  eventTypeDisplayName?: null | string;
  maskedPayload?: null | any;
  timeStamp?: string;
  version?: number;
}
