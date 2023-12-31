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

import { CreateRedirection } from '../models/create-redirection';
import { DeleteRedirection } from '../models/delete-redirection';
import { EditRedirection } from '../models/edit-redirection';
import { RedirectionsListDto } from '../models/redirections-list-dto';

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
   * Path part for operation apiRedirectionsGet
   */
  static readonly ApiRedirectionsGetPath = '/api/redirections';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiRedirectionsGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiRedirectionsGet$Plain$Response(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<RedirectionsListDto>> {

    const rb = new RequestBuilder(this.rootUrl, RedirectionsService.ApiRedirectionsGetPath, 'get');
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
        return r as StrictHttpResponse<RedirectionsListDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiRedirectionsGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiRedirectionsGet$Plain(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<RedirectionsListDto> {

    return this.apiRedirectionsGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<RedirectionsListDto>) => r.body as RedirectionsListDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiRedirectionsGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiRedirectionsGet$Json$Response(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<RedirectionsListDto>> {

    const rb = new RequestBuilder(this.rootUrl, RedirectionsService.ApiRedirectionsGetPath, 'get');
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
        return r as StrictHttpResponse<RedirectionsListDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiRedirectionsGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiRedirectionsGet$Json(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<RedirectionsListDto> {

    return this.apiRedirectionsGet$Json$Response(params,context).pipe(
      map((r: StrictHttpResponse<RedirectionsListDto>) => r.body as RedirectionsListDto)
    );
  }

  /**
   * Path part for operation apiRedirectionsPost
   */
  static readonly ApiRedirectionsPostPath = '/api/redirections';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiRedirectionsPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiRedirectionsPost$Response(params?: {
    body?: CreateRedirection
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, RedirectionsService.ApiRedirectionsPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiRedirectionsPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiRedirectionsPost(params?: {
    body?: CreateRedirection
  },
  context?: HttpContext

): Observable<void> {

    return this.apiRedirectionsPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiRedirectionsDelete
   */
  static readonly ApiRedirectionsDeletePath = '/api/redirections';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiRedirectionsDelete()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiRedirectionsDelete$Response(params?: {
    body?: DeleteRedirection
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, RedirectionsService.ApiRedirectionsDeletePath, 'delete');
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
   * To access the full response (for headers, for example), `apiRedirectionsDelete$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiRedirectionsDelete(params?: {
    body?: DeleteRedirection
  },
  context?: HttpContext

): Observable<void> {

    return this.apiRedirectionsDelete$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiRedirectionsPatch
   */
  static readonly ApiRedirectionsPatchPath = '/api/redirections';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiRedirectionsPatch()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiRedirectionsPatch$Response(params?: {
    body?: EditRedirection
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, RedirectionsService.ApiRedirectionsPatchPath, 'patch');
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
   * To access the full response (for headers, for example), `apiRedirectionsPatch$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiRedirectionsPatch(params?: {
    body?: EditRedirection
  },
  context?: HttpContext

): Observable<void> {

    return this.apiRedirectionsPatch$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
