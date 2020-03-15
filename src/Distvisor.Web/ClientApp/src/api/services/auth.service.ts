/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

import { AuthUser } from '../models/auth-user';
import { LoginRequestDto } from '../models/login-request-dto';
import { RefreshTokenDto } from '../models/refresh-token-dto';

@Injectable({
  providedIn: 'root',
})
export class AuthService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiAuthGet
   */
  static readonly ApiAuthGetPath = '/api/Auth';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAuthGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAuthGet$Response(params?: {

  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AuthService.ApiAuthGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiAuthGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAuthGet(params?: {

  }): Observable<void> {

    return this.apiAuthGet$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiAuthLoginPost
   */
  static readonly ApiAuthLoginPostPath = '/api/Auth/login';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAuthLoginPost$Json$Plain()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  apiAuthLoginPost$Json$Plain$Response(params?: {

    body?: LoginRequestDto
  }): Observable<StrictHttpResponse<AuthUser>> {

    const rb = new RequestBuilder(this.rootUrl, AuthService.ApiAuthLoginPostPath, 'post');
    if (params) {


      rb.body(params.body, 'application/json');
    }
    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<AuthUser>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiAuthLoginPost$Json$Plain$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  apiAuthLoginPost$Json$Plain(params?: {

    body?: LoginRequestDto
  }): Observable<AuthUser> {

    return this.apiAuthLoginPost$Json$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<AuthUser>) => r.body as AuthUser)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAuthLoginPost$Json$Json()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  apiAuthLoginPost$Json$Json$Response(params?: {

    body?: LoginRequestDto
  }): Observable<StrictHttpResponse<AuthUser>> {

    const rb = new RequestBuilder(this.rootUrl, AuthService.ApiAuthLoginPostPath, 'post');
    if (params) {


      rb.body(params.body, 'application/json');
    }
    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'application/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<AuthUser>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiAuthLoginPost$Json$Json$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  apiAuthLoginPost$Json$Json(params?: {

    body?: LoginRequestDto
  }): Observable<AuthUser> {

    return this.apiAuthLoginPost$Json$Json$Response(params).pipe(
      map((r: StrictHttpResponse<AuthUser>) => r.body as AuthUser)
    );
  }

  /**
   * Path part for operation apiAuthRefreshtokenPost
   */
  static readonly ApiAuthRefreshtokenPostPath = '/api/Auth/refreshtoken';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAuthRefreshtokenPost$Json$Plain()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  apiAuthRefreshtokenPost$Json$Plain$Response(params?: {

    body?: RefreshTokenDto
  }): Observable<StrictHttpResponse<AuthUser>> {

    const rb = new RequestBuilder(this.rootUrl, AuthService.ApiAuthRefreshtokenPostPath, 'post');
    if (params) {


      rb.body(params.body, 'application/json');
    }
    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<AuthUser>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiAuthRefreshtokenPost$Json$Plain$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  apiAuthRefreshtokenPost$Json$Plain(params?: {

    body?: RefreshTokenDto
  }): Observable<AuthUser> {

    return this.apiAuthRefreshtokenPost$Json$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<AuthUser>) => r.body as AuthUser)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAuthRefreshtokenPost$Json$Json()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  apiAuthRefreshtokenPost$Json$Json$Response(params?: {

    body?: RefreshTokenDto
  }): Observable<StrictHttpResponse<AuthUser>> {

    const rb = new RequestBuilder(this.rootUrl, AuthService.ApiAuthRefreshtokenPostPath, 'post');
    if (params) {


      rb.body(params.body, 'application/json');
    }
    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'application/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<AuthUser>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiAuthRefreshtokenPost$Json$Json$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  apiAuthRefreshtokenPost$Json$Json(params?: {

    body?: RefreshTokenDto
  }): Observable<AuthUser> {

    return this.apiAuthRefreshtokenPost$Json$Json$Response(params).pipe(
      map((r: StrictHttpResponse<AuthUser>) => r.body as AuthUser)
    );
  }

  /**
   * Path part for operation apiAuthLogoutPost
   */
  static readonly ApiAuthLogoutPostPath = '/api/Auth/logout';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAuthLogoutPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAuthLogoutPost$Response(params?: {

  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AuthService.ApiAuthLogoutPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiAuthLogoutPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAuthLogoutPost(params?: {

  }): Observable<void> {

    return this.apiAuthLogoutPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
