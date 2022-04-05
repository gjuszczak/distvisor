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


@Injectable({
  providedIn: 'root',
})
export class CoreService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiCoreApiLoginPost
   */
  static readonly ApiCoreApiLoginPostPath = '/api/core/api-login';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiCoreApiLoginPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiCoreApiLoginPost$Response(params?: {
    username?: string;
    password?: string;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, CoreService.ApiCoreApiLoginPostPath, 'post');
    if (params) {
      rb.query('username', params.username, {});
      rb.query('password', params.password, {});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: '*/*'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiCoreApiLoginPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiCoreApiLoginPost(params?: {
    username?: string;
    password?: string;
  }): Observable<void> {

    return this.apiCoreApiLoginPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiCoreApiRefreshPost
   */
  static readonly ApiCoreApiRefreshPostPath = '/api/core/api-refresh';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiCoreApiRefreshPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiCoreApiRefreshPost$Response(params?: {
    sessionId?: string;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, CoreService.ApiCoreApiRefreshPostPath, 'post');
    if (params) {
      rb.query('sessionId', params.sessionId, {});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: '*/*'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiCoreApiRefreshPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiCoreApiRefreshPost(params?: {
    sessionId?: string;
  }): Observable<void> {

    return this.apiCoreApiRefreshPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiCoreSyncDevicesPost
   */
  static readonly ApiCoreSyncDevicesPostPath = '/api/core/sync-devices';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiCoreSyncDevicesPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiCoreSyncDevicesPost$Response(params?: {
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, CoreService.ApiCoreSyncDevicesPostPath, 'post');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: '*/*'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiCoreSyncDevicesPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiCoreSyncDevicesPost(params?: {
  }): Observable<void> {

    return this.apiCoreSyncDevicesPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiCoreDevicesGet
   */
  static readonly ApiCoreDevicesGetPath = '/api/core/devices';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiCoreDevicesGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiCoreDevicesGet$Response(params?: {
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, CoreService.ApiCoreDevicesGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: '*/*'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiCoreDevicesGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiCoreDevicesGet(params?: {
  }): Observable<void> {

    return this.apiCoreDevicesGet$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
