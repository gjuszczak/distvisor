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

import { UserInfoDto } from '../models/user-info-dto';

@Injectable({
  providedIn: 'root',
})
export class AccountService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiSecAccountGet
   */
  static readonly ApiSecAccountGetPath = '/api/sec/Account';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecAccountGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAccountGet$Plain$Response(params?: {
  },
  context?: HttpContext

): Observable<StrictHttpResponse<UserInfoDto>> {

    const rb = new RequestBuilder(this.rootUrl, AccountService.ApiSecAccountGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<UserInfoDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiSecAccountGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAccountGet$Plain(params?: {
  },
  context?: HttpContext

): Observable<UserInfoDto> {

    return this.apiSecAccountGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<UserInfoDto>) => r.body as UserInfoDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecAccountGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAccountGet$Json$Response(params?: {
  },
  context?: HttpContext

): Observable<StrictHttpResponse<UserInfoDto>> {

    const rb = new RequestBuilder(this.rootUrl, AccountService.ApiSecAccountGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<UserInfoDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiSecAccountGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAccountGet$Json(params?: {
  },
  context?: HttpContext

): Observable<UserInfoDto> {

    return this.apiSecAccountGet$Json$Response(params,context).pipe(
      map((r: StrictHttpResponse<UserInfoDto>) => r.body as UserInfoDto)
    );
  }

}
