/* tslint:disable */
/* eslint-disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

import { AggregateDto } from '../models/aggregate-dto';
import { EventsListDto } from '../models/events-list-dto';

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
   * Path part for operation apiSEventLogGet
   */
  static readonly ApiSEventLogGetPath = '/api/s/event-log';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSEventLogGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSEventLogGet$Plain$Response(params?: {
    aggregateId?: string;
    first?: number;
    rows?: number;
  }): Observable<StrictHttpResponse<EventsListDto>> {

    const rb = new RequestBuilder(this.rootUrl, EventLogService.ApiSEventLogGetPath, 'get');
    if (params) {
      rb.query('aggregateId', params.aggregateId, {});
      rb.query('first', params.first, {});
      rb.query('rows', params.rows, {});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<EventsListDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSEventLogGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSEventLogGet$Plain(params?: {
    aggregateId?: string;
    first?: number;
    rows?: number;
  }): Observable<EventsListDto> {

    return this.apiSEventLogGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<EventsListDto>) => r.body as EventsListDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSEventLogGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSEventLogGet$Json$Response(params?: {
    aggregateId?: string;
    first?: number;
    rows?: number;
  }): Observable<StrictHttpResponse<EventsListDto>> {

    const rb = new RequestBuilder(this.rootUrl, EventLogService.ApiSEventLogGetPath, 'get');
    if (params) {
      rb.query('aggregateId', params.aggregateId, {});
      rb.query('first', params.first, {});
      rb.query('rows', params.rows, {});
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<EventsListDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSEventLogGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSEventLogGet$Json(params?: {
    aggregateId?: string;
    first?: number;
    rows?: number;
  }): Observable<EventsListDto> {

    return this.apiSEventLogGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<EventsListDto>) => r.body as EventsListDto)
    );
  }

  /**
   * Path part for operation apiSEventLogAggregatesAggregateIdGet
   */
  static readonly ApiSEventLogAggregatesAggregateIdGetPath = '/api/s/event-log/aggregates/{aggregateId}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSEventLogAggregatesAggregateIdGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSEventLogAggregatesAggregateIdGet$Plain$Response(params: {
    aggregateId: string;
  }): Observable<StrictHttpResponse<AggregateDto>> {

    const rb = new RequestBuilder(this.rootUrl, EventLogService.ApiSEventLogAggregatesAggregateIdGetPath, 'get');
    if (params) {
      rb.path('aggregateId', params.aggregateId, {});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<AggregateDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSEventLogAggregatesAggregateIdGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSEventLogAggregatesAggregateIdGet$Plain(params: {
    aggregateId: string;
  }): Observable<AggregateDto> {

    return this.apiSEventLogAggregatesAggregateIdGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<AggregateDto>) => r.body as AggregateDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSEventLogAggregatesAggregateIdGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSEventLogAggregatesAggregateIdGet$Json$Response(params: {
    aggregateId: string;
  }): Observable<StrictHttpResponse<AggregateDto>> {

    const rb = new RequestBuilder(this.rootUrl, EventLogService.ApiSEventLogAggregatesAggregateIdGetPath, 'get');
    if (params) {
      rb.path('aggregateId', params.aggregateId, {});
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<AggregateDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSEventLogAggregatesAggregateIdGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSEventLogAggregatesAggregateIdGet$Json(params: {
    aggregateId: string;
  }): Observable<AggregateDto> {

    return this.apiSEventLogAggregatesAggregateIdGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<AggregateDto>) => r.body as AggregateDto)
    );
  }

}
