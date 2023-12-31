/* tslint:disable */
/* eslint-disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpContext } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

import { AggregateDto } from '../models/aggregate-dto';
import { EventsListDto } from '../models/events-list-dto';
import { ReplayEvents } from '../models/replay-events';

@Injectable({
  providedIn: 'root',
})
export class EventLogService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiEventLogEventsGet
   */
  static readonly ApiEventLogEventsGetPath = '/api/event-log/events';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiEventLogEventsGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiEventLogEventsGet$Plain$Response(params?: {
    aggregateId?: string;
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<EventsListDto>> {

    const rb = new RequestBuilder(this.rootUrl, EventLogService.ApiEventLogEventsGetPath, 'get');
    if (params) {
      rb.query('aggregateId', params.aggregateId, {});
      rb.query('first', params.first, {});
      rb.query('rows', params.rows, {});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<EventsListDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiEventLogEventsGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiEventLogEventsGet$Plain(params?: {
    aggregateId?: string;
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<EventsListDto> {

    return this.apiEventLogEventsGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<EventsListDto>) => r.body as EventsListDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiEventLogEventsGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiEventLogEventsGet$Json$Response(params?: {
    aggregateId?: string;
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<EventsListDto>> {

    const rb = new RequestBuilder(this.rootUrl, EventLogService.ApiEventLogEventsGetPath, 'get');
    if (params) {
      rb.query('aggregateId', params.aggregateId, {});
      rb.query('first', params.first, {});
      rb.query('rows', params.rows, {});
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<EventsListDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiEventLogEventsGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiEventLogEventsGet$Json(params?: {
    aggregateId?: string;
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<EventsListDto> {

    return this.apiEventLogEventsGet$Json$Response(params,context).pipe(
      map((r: StrictHttpResponse<EventsListDto>) => r.body as EventsListDto)
    );
  }

  /**
   * Path part for operation apiEventLogAggregatesAggregateIdGet
   */
  static readonly ApiEventLogAggregatesAggregateIdGetPath = '/api/event-log/aggregates/{aggregateId}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiEventLogAggregatesAggregateIdGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiEventLogAggregatesAggregateIdGet$Plain$Response(params: {
    aggregateId: string;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<AggregateDto>> {

    const rb = new RequestBuilder(this.rootUrl, EventLogService.ApiEventLogAggregatesAggregateIdGetPath, 'get');
    if (params) {
      rb.path('aggregateId', params.aggregateId, {});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<AggregateDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiEventLogAggregatesAggregateIdGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiEventLogAggregatesAggregateIdGet$Plain(params: {
    aggregateId: string;
  },
  context?: HttpContext

): Observable<AggregateDto> {

    return this.apiEventLogAggregatesAggregateIdGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<AggregateDto>) => r.body as AggregateDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiEventLogAggregatesAggregateIdGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiEventLogAggregatesAggregateIdGet$Json$Response(params: {
    aggregateId: string;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<AggregateDto>> {

    const rb = new RequestBuilder(this.rootUrl, EventLogService.ApiEventLogAggregatesAggregateIdGetPath, 'get');
    if (params) {
      rb.path('aggregateId', params.aggregateId, {});
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<AggregateDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiEventLogAggregatesAggregateIdGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiEventLogAggregatesAggregateIdGet$Json(params: {
    aggregateId: string;
  },
  context?: HttpContext

): Observable<AggregateDto> {

    return this.apiEventLogAggregatesAggregateIdGet$Json$Response(params,context).pipe(
      map((r: StrictHttpResponse<AggregateDto>) => r.body as AggregateDto)
    );
  }

  /**
   * Path part for operation apiEventLogReplayEventsPost
   */
  static readonly ApiEventLogReplayEventsPostPath = '/api/event-log/replay-events';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiEventLogReplayEventsPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiEventLogReplayEventsPost$Response(params?: {
    body?: ReplayEvents
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, EventLogService.ApiEventLogReplayEventsPostPath, 'post');
    if (params) {
      rb.body(params.body, 'application/*+json');
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: '*/*',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiEventLogReplayEventsPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiEventLogReplayEventsPost(params?: {
    body?: ReplayEvents
  },
  context?: HttpContext

): Observable<void> {

    return this.apiEventLogReplayEventsPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
