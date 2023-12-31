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

import { DeleteGatewaySession } from '../models/delete-gateway-session';
import { DevicesListDto } from '../models/devices-list-dto';
import { GatewaySessionsListDto } from '../models/gateway-sessions-list-dto';
import { OpenGatewaySession } from '../models/open-gateway-session';
import { RefreshGatewaySession } from '../models/refresh-gateway-session';

@Injectable({
  providedIn: 'root',
})
export class HomeBoxService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiHomeBoxSessionsGet
   */
  static readonly ApiHomeBoxSessionsGetPath = '/api/home-box/sessions';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiHomeBoxSessionsGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiHomeBoxSessionsGet$Plain$Response(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<GatewaySessionsListDto>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiHomeBoxSessionsGetPath, 'get');
    if (params) {
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
        return r as StrictHttpResponse<GatewaySessionsListDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiHomeBoxSessionsGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiHomeBoxSessionsGet$Plain(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<GatewaySessionsListDto> {

    return this.apiHomeBoxSessionsGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<GatewaySessionsListDto>) => r.body as GatewaySessionsListDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiHomeBoxSessionsGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiHomeBoxSessionsGet$Json$Response(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<GatewaySessionsListDto>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiHomeBoxSessionsGetPath, 'get');
    if (params) {
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
        return r as StrictHttpResponse<GatewaySessionsListDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiHomeBoxSessionsGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiHomeBoxSessionsGet$Json(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<GatewaySessionsListDto> {

    return this.apiHomeBoxSessionsGet$Json$Response(params,context).pipe(
      map((r: StrictHttpResponse<GatewaySessionsListDto>) => r.body as GatewaySessionsListDto)
    );
  }

  /**
   * Path part for operation apiHomeBoxSessionsOpenPost
   */
  static readonly ApiHomeBoxSessionsOpenPostPath = '/api/home-box/sessions/open';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiHomeBoxSessionsOpenPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiHomeBoxSessionsOpenPost$Response(params?: {
    body?: OpenGatewaySession
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiHomeBoxSessionsOpenPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiHomeBoxSessionsOpenPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiHomeBoxSessionsOpenPost(params?: {
    body?: OpenGatewaySession
  },
  context?: HttpContext

): Observable<void> {

    return this.apiHomeBoxSessionsOpenPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiHomeBoxSessionsRefreshPost
   */
  static readonly ApiHomeBoxSessionsRefreshPostPath = '/api/home-box/sessions/refresh';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiHomeBoxSessionsRefreshPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiHomeBoxSessionsRefreshPost$Response(params?: {
    body?: RefreshGatewaySession
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiHomeBoxSessionsRefreshPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiHomeBoxSessionsRefreshPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiHomeBoxSessionsRefreshPost(params?: {
    body?: RefreshGatewaySession
  },
  context?: HttpContext

): Observable<void> {

    return this.apiHomeBoxSessionsRefreshPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiHomeBoxSessionsDeletePost
   */
  static readonly ApiHomeBoxSessionsDeletePostPath = '/api/home-box/sessions/delete';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiHomeBoxSessionsDeletePost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiHomeBoxSessionsDeletePost$Response(params?: {
    body?: DeleteGatewaySession
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiHomeBoxSessionsDeletePostPath, 'post');
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
   * To access the full response (for headers, for example), `apiHomeBoxSessionsDeletePost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiHomeBoxSessionsDeletePost(params?: {
    body?: DeleteGatewaySession
  },
  context?: HttpContext

): Observable<void> {

    return this.apiHomeBoxSessionsDeletePost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiHomeBoxDevicesGet
   */
  static readonly ApiHomeBoxDevicesGetPath = '/api/home-box/devices';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiHomeBoxDevicesGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiHomeBoxDevicesGet$Plain$Response(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<DevicesListDto>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiHomeBoxDevicesGetPath, 'get');
    if (params) {
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
        return r as StrictHttpResponse<DevicesListDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiHomeBoxDevicesGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiHomeBoxDevicesGet$Plain(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<DevicesListDto> {

    return this.apiHomeBoxDevicesGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<DevicesListDto>) => r.body as DevicesListDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiHomeBoxDevicesGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiHomeBoxDevicesGet$Json$Response(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<DevicesListDto>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiHomeBoxDevicesGetPath, 'get');
    if (params) {
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
        return r as StrictHttpResponse<DevicesListDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiHomeBoxDevicesGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiHomeBoxDevicesGet$Json(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<DevicesListDto> {

    return this.apiHomeBoxDevicesGet$Json$Response(params,context).pipe(
      map((r: StrictHttpResponse<DevicesListDto>) => r.body as DevicesListDto)
    );
  }

  /**
   * Path part for operation apiHomeBoxDevicesSyncWithGatewayPost
   */
  static readonly ApiHomeBoxDevicesSyncWithGatewayPostPath = '/api/home-box/devices/sync-with-gateway';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiHomeBoxDevicesSyncWithGatewayPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiHomeBoxDevicesSyncWithGatewayPost$Response(params?: {
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiHomeBoxDevicesSyncWithGatewayPostPath, 'post');
    if (params) {
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
   * To access the full response (for headers, for example), `apiHomeBoxDevicesSyncWithGatewayPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiHomeBoxDevicesSyncWithGatewayPost(params?: {
  },
  context?: HttpContext

): Observable<void> {

    return this.apiHomeBoxDevicesSyncWithGatewayPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
