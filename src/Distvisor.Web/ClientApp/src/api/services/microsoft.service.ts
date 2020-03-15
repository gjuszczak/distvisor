/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

import { MicrosoftAuthDto } from '../models/microsoft-auth-dto';

@Injectable({
  providedIn: 'root',
})
export class MicrosoftService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiMicrosoftAuthUriGet
   */
  static readonly ApiMicrosoftAuthUriGetPath = '/api/Microsoft/auth-uri';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiMicrosoftAuthUriGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiMicrosoftAuthUriGet$Plain$Response(params?: {

  }): Observable<StrictHttpResponse<MicrosoftAuthDto>> {

    const rb = new RequestBuilder(this.rootUrl, MicrosoftService.ApiMicrosoftAuthUriGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<MicrosoftAuthDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiMicrosoftAuthUriGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiMicrosoftAuthUriGet$Plain(params?: {

  }): Observable<MicrosoftAuthDto> {

    return this.apiMicrosoftAuthUriGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<MicrosoftAuthDto>) => r.body as MicrosoftAuthDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiMicrosoftAuthUriGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiMicrosoftAuthUriGet$Json$Response(params?: {

  }): Observable<StrictHttpResponse<MicrosoftAuthDto>> {

    const rb = new RequestBuilder(this.rootUrl, MicrosoftService.ApiMicrosoftAuthUriGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'application/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<MicrosoftAuthDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiMicrosoftAuthUriGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiMicrosoftAuthUriGet$Json(params?: {

  }): Observable<MicrosoftAuthDto> {

    return this.apiMicrosoftAuthUriGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<MicrosoftAuthDto>) => r.body as MicrosoftAuthDto)
    );
  }

  /**
   * Path part for operation apiMicrosoftAuthRedirectGet
   */
  static readonly ApiMicrosoftAuthRedirectGetPath = '/api/Microsoft/auth-redirect';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiMicrosoftAuthRedirectGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiMicrosoftAuthRedirectGet$Response(params?: {
    code?: string;
    state?: string;

  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, MicrosoftService.ApiMicrosoftAuthRedirectGetPath, 'get');
    if (params) {

      rb.query('code', params.code);
      rb.query('state', params.state);

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
   * To access the full response (for headers, for example), `apiMicrosoftAuthRedirectGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiMicrosoftAuthRedirectGet(params?: {
    code?: string;
    state?: string;

  }): Observable<void> {

    return this.apiMicrosoftAuthRedirectGet$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiMicrosoftBackupGet
   */
  static readonly ApiMicrosoftBackupGetPath = '/api/Microsoft/backup';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiMicrosoftBackupGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiMicrosoftBackupGet$Response(params?: {

  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, MicrosoftService.ApiMicrosoftBackupGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiMicrosoftBackupGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiMicrosoftBackupGet(params?: {

  }): Observable<void> {

    return this.apiMicrosoftBackupGet$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
