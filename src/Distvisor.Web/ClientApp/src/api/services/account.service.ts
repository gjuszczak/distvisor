/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

import { AccountInfoDto } from '../models/account-info-dto';

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
   * Path part for operation apiAccountGet
   */
  static readonly ApiAccountGetPath = '/api/Account';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAccountGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAccountGet$Plain$Response(params?: {

  }): Observable<StrictHttpResponse<AccountInfoDto>> {

    const rb = new RequestBuilder(this.rootUrl, AccountService.ApiAccountGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<AccountInfoDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiAccountGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAccountGet$Plain(params?: {

  }): Observable<AccountInfoDto> {

    return this.apiAccountGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<AccountInfoDto>) => r.body as AccountInfoDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAccountGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAccountGet$Json$Response(params?: {

  }): Observable<StrictHttpResponse<AccountInfoDto>> {

    const rb = new RequestBuilder(this.rootUrl, AccountService.ApiAccountGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'application/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<AccountInfoDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiAccountGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAccountGet$Json(params?: {

  }): Observable<AccountInfoDto> {

    return this.apiAccountGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<AccountInfoDto>) => r.body as AccountInfoDto)
    );
  }

}