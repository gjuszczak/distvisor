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

import { ClientConfiguration } from '../models/client-configuration';

@Injectable({
  providedIn: 'root',
})
export class ClientConfigService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiClientConfigGet
   */
  static readonly ApiClientConfigGetPath = '/api/ClientConfig';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiClientConfigGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiClientConfigGet$Plain$Response(params?: {
  }): Observable<StrictHttpResponse<ClientConfiguration>> {

    const rb = new RequestBuilder(this.rootUrl, ClientConfigService.ApiClientConfigGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<ClientConfiguration>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiClientConfigGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiClientConfigGet$Plain(params?: {
  }): Observable<ClientConfiguration> {

    return this.apiClientConfigGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<ClientConfiguration>) => r.body as ClientConfiguration)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiClientConfigGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiClientConfigGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<ClientConfiguration>> {

    const rb = new RequestBuilder(this.rootUrl, ClientConfigService.ApiClientConfigGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<ClientConfiguration>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiClientConfigGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiClientConfigGet$Json(params?: {
  }): Observable<ClientConfiguration> {

    return this.apiClientConfigGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<ClientConfiguration>) => r.body as ClientConfiguration)
    );
  }

}
