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

import { EventLogDto } from '../models/event-log-dto';

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
   * Path part for operation apiSecEventLogListGet
   */
  static readonly ApiSecEventLogListGetPath = '/api/sec/EventLog/list';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecEventLogListGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecEventLogListGet$Plain$Response(params?: {
  }): Observable<StrictHttpResponse<Array<EventLogDto>>> {

    const rb = new RequestBuilder(this.rootUrl, EventLogService.ApiSecEventLogListGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<EventLogDto>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSecEventLogListGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecEventLogListGet$Plain(params?: {
  }): Observable<Array<EventLogDto>> {

    return this.apiSecEventLogListGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<Array<EventLogDto>>) => r.body as Array<EventLogDto>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecEventLogListGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecEventLogListGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<Array<EventLogDto>>> {

    const rb = new RequestBuilder(this.rootUrl, EventLogService.ApiSecEventLogListGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<EventLogDto>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSecEventLogListGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecEventLogListGet$Json(params?: {
  }): Observable<Array<EventLogDto>> {

    return this.apiSecEventLogListGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<Array<EventLogDto>>) => r.body as Array<EventLogDto>)
    );
  }

}
