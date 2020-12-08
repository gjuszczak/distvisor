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

import { RedirectionDetails } from '../models/redirection-details';

@Injectable({
  providedIn: 'root',
})
export class RedirectionsService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiRedirectionsNameGet
   */
  static readonly ApiRedirectionsNameGetPath = '/api/Redirections/{name}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiRedirectionsNameGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiRedirectionsNameGet$Response(params: {
    name: string;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, RedirectionsService.ApiRedirectionsNameGetPath, 'get');
    if (params) {
      rb.path('name', params.name, {});
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
   * To access the full response (for headers, for example), `apiRedirectionsNameGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiRedirectionsNameGet(params: {
    name: string;
  }): Observable<void> {

    return this.apiRedirectionsNameGet$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiRedirectionsNameDelete
   */
  static readonly ApiRedirectionsNameDeletePath = '/api/Redirections/{name}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiRedirectionsNameDelete()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiRedirectionsNameDelete$Response(params: {
    name: string;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, RedirectionsService.ApiRedirectionsNameDeletePath, 'delete');
    if (params) {
      rb.path('name', params.name, {});
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
   * To access the full response (for headers, for example), `apiRedirectionsNameDelete$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiRedirectionsNameDelete(params: {
    name: string;
  }): Observable<void> {

    return this.apiRedirectionsNameDelete$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiRedirectionsGet
   */
  static readonly ApiRedirectionsGetPath = '/api/Redirections';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiRedirectionsGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiRedirectionsGet$Plain$Response(params?: {
  }): Observable<StrictHttpResponse<Array<RedirectionDetails>>> {

    const rb = new RequestBuilder(this.rootUrl, RedirectionsService.ApiRedirectionsGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<RedirectionDetails>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiRedirectionsGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiRedirectionsGet$Plain(params?: {
  }): Observable<Array<RedirectionDetails>> {

    return this.apiRedirectionsGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<Array<RedirectionDetails>>) => r.body as Array<RedirectionDetails>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiRedirectionsGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiRedirectionsGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<Array<RedirectionDetails>>> {

    const rb = new RequestBuilder(this.rootUrl, RedirectionsService.ApiRedirectionsGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<RedirectionDetails>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiRedirectionsGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiRedirectionsGet$Json(params?: {
  }): Observable<Array<RedirectionDetails>> {

    return this.apiRedirectionsGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<Array<RedirectionDetails>>) => r.body as Array<RedirectionDetails>)
    );
  }

  /**
   * Path part for operation apiRedirectionsPost
   */
  static readonly ApiRedirectionsPostPath = '/api/Redirections';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiRedirectionsPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiRedirectionsPost$Response(params?: {
    body?: RedirectionDetails
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, RedirectionsService.ApiRedirectionsPostPath, 'post');
    if (params) {
      rb.body(params.body, 'application/*+json');
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
   * To access the full response (for headers, for example), `apiRedirectionsPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiRedirectionsPost(params?: {
    body?: RedirectionDetails
  }): Observable<void> {

    return this.apiRedirectionsPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
