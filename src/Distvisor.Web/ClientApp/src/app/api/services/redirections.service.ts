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
   * Path part for operation apiSecRedirectionsNameDelete
   */
  static readonly ApiSecRedirectionsNameDeletePath = '/api/sec/Redirections/{name}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecRedirectionsNameDelete()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecRedirectionsNameDelete$Response(params: {
    name: string;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, RedirectionsService.ApiSecRedirectionsNameDeletePath, 'delete');
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
   * To access the full response (for headers, for example), `apiSecRedirectionsNameDelete$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecRedirectionsNameDelete(params: {
    name: string;
  }): Observable<void> {

    return this.apiSecRedirectionsNameDelete$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecRedirectionsGet
   */
  static readonly ApiSecRedirectionsGetPath = '/api/sec/Redirections';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecRedirectionsGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecRedirectionsGet$Plain$Response(params?: {
  }): Observable<StrictHttpResponse<Array<RedirectionDetails>>> {

    const rb = new RequestBuilder(this.rootUrl, RedirectionsService.ApiSecRedirectionsGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiSecRedirectionsGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecRedirectionsGet$Plain(params?: {
  }): Observable<Array<RedirectionDetails>> {

    return this.apiSecRedirectionsGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<Array<RedirectionDetails>>) => r.body as Array<RedirectionDetails>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecRedirectionsGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecRedirectionsGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<Array<RedirectionDetails>>> {

    const rb = new RequestBuilder(this.rootUrl, RedirectionsService.ApiSecRedirectionsGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiSecRedirectionsGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecRedirectionsGet$Json(params?: {
  }): Observable<Array<RedirectionDetails>> {

    return this.apiSecRedirectionsGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<Array<RedirectionDetails>>) => r.body as Array<RedirectionDetails>)
    );
  }

  /**
   * Path part for operation apiSecRedirectionsPost
   */
  static readonly ApiSecRedirectionsPostPath = '/api/sec/Redirections';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecRedirectionsPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecRedirectionsPost$Response(params?: {
    body?: RedirectionDetails
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, RedirectionsService.ApiSecRedirectionsPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSecRedirectionsPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecRedirectionsPost(params?: {
    body?: RedirectionDetails
  }): Observable<void> {

    return this.apiSecRedirectionsPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
