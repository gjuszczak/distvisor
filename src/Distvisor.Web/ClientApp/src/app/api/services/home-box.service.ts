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
import { DeviceDto } from '../models/device-dto';
import { GatewaySessionDtoPaginatedList } from '../models/gateway-session-dto-paginated-list';
import { GetDevices } from '../models/get-devices';
import { LoginToGateway } from '../models/login-to-gateway';
import { RefreshGatewaySession } from '../models/refresh-gateway-session';
import { SyncDevicesWithGateway } from '../models/sync-devices-with-gateway';

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
   * Path part for operation apiSHomeBoxGatewaySessionsGet
   */
  static readonly ApiSHomeBoxGatewaySessionsGetPath = '/api/s/home-box/gateway-sessions';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSHomeBoxGatewaySessionsGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSHomeBoxGatewaySessionsGet$Plain$Response(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<GatewaySessionDtoPaginatedList>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSHomeBoxGatewaySessionsGetPath, 'get');
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
        return r as StrictHttpResponse<GatewaySessionDtoPaginatedList>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiSHomeBoxGatewaySessionsGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSHomeBoxGatewaySessionsGet$Plain(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<GatewaySessionDtoPaginatedList> {

    return this.apiSHomeBoxGatewaySessionsGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<GatewaySessionDtoPaginatedList>) => r.body as GatewaySessionDtoPaginatedList)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSHomeBoxGatewaySessionsGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSHomeBoxGatewaySessionsGet$Json$Response(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<GatewaySessionDtoPaginatedList>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSHomeBoxGatewaySessionsGetPath, 'get');
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
        return r as StrictHttpResponse<GatewaySessionDtoPaginatedList>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiSHomeBoxGatewaySessionsGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSHomeBoxGatewaySessionsGet$Json(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<GatewaySessionDtoPaginatedList> {

    return this.apiSHomeBoxGatewaySessionsGet$Json$Response(params,context).pipe(
      map((r: StrictHttpResponse<GatewaySessionDtoPaginatedList>) => r.body as GatewaySessionDtoPaginatedList)
    );
  }

  /**
   * Path part for operation apiSHomeBoxLoginToGatewayPost
   */
  static readonly ApiSHomeBoxLoginToGatewayPostPath = '/api/s/home-box/login-to-gateway';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSHomeBoxLoginToGatewayPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSHomeBoxLoginToGatewayPost$Response(params?: {
    body?: LoginToGateway
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSHomeBoxLoginToGatewayPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSHomeBoxLoginToGatewayPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSHomeBoxLoginToGatewayPost(params?: {
    body?: LoginToGateway
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSHomeBoxLoginToGatewayPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSHomeBoxRefreshGatewaySessionPost
   */
  static readonly ApiSHomeBoxRefreshGatewaySessionPostPath = '/api/s/home-box/refresh-gateway-session';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSHomeBoxRefreshGatewaySessionPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSHomeBoxRefreshGatewaySessionPost$Response(params?: {
    body?: RefreshGatewaySession
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSHomeBoxRefreshGatewaySessionPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSHomeBoxRefreshGatewaySessionPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSHomeBoxRefreshGatewaySessionPost(params?: {
    body?: RefreshGatewaySession
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSHomeBoxRefreshGatewaySessionPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSHomeBoxDeleteGatewaySessionPost
   */
  static readonly ApiSHomeBoxDeleteGatewaySessionPostPath = '/api/s/home-box/delete-gateway-session';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSHomeBoxDeleteGatewaySessionPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSHomeBoxDeleteGatewaySessionPost$Response(params?: {
    body?: DeleteGatewaySession
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSHomeBoxDeleteGatewaySessionPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSHomeBoxDeleteGatewaySessionPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSHomeBoxDeleteGatewaySessionPost(params?: {
    body?: DeleteGatewaySession
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSHomeBoxDeleteGatewaySessionPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSHomeBoxSyncDevicesWithGatewayPost
   */
  static readonly ApiSHomeBoxSyncDevicesWithGatewayPostPath = '/api/s/home-box/sync-devices-with-gateway';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSHomeBoxSyncDevicesWithGatewayPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSHomeBoxSyncDevicesWithGatewayPost$Response(params?: {
    body?: SyncDevicesWithGateway
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSHomeBoxSyncDevicesWithGatewayPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSHomeBoxSyncDevicesWithGatewayPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSHomeBoxSyncDevicesWithGatewayPost(params?: {
    body?: SyncDevicesWithGateway
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSHomeBoxSyncDevicesWithGatewayPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSHomeBoxDevicesGet
   */
  static readonly ApiSHomeBoxDevicesGetPath = '/api/s/home-box/devices';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSHomeBoxDevicesGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSHomeBoxDevicesGet$Plain$Response(params?: {
    query?: GetDevices;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<Array<DeviceDto>>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSHomeBoxDevicesGetPath, 'get');
    if (params) {
      rb.query('query', params.query, {});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<DeviceDto>>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiSHomeBoxDevicesGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSHomeBoxDevicesGet$Plain(params?: {
    query?: GetDevices;
  },
  context?: HttpContext

): Observable<Array<DeviceDto>> {

    return this.apiSHomeBoxDevicesGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<Array<DeviceDto>>) => r.body as Array<DeviceDto>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSHomeBoxDevicesGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSHomeBoxDevicesGet$Json$Response(params?: {
    query?: GetDevices;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<Array<DeviceDto>>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSHomeBoxDevicesGetPath, 'get');
    if (params) {
      rb.query('query', params.query, {});
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<DeviceDto>>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiSHomeBoxDevicesGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSHomeBoxDevicesGet$Json(params?: {
    query?: GetDevices;
  },
  context?: HttpContext

): Observable<Array<DeviceDto>> {

    return this.apiSHomeBoxDevicesGet$Json$Response(params,context).pipe(
      map((r: StrictHttpResponse<Array<DeviceDto>>) => r.body as Array<DeviceDto>)
    );
  }

}
