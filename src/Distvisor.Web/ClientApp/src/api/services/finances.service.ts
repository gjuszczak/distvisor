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

import { FinancialAccountDto } from '../models/financial-account-dto';
import { FinancialAccountTransactionDto } from '../models/financial-account-transaction-dto';

@Injectable({
  providedIn: 'root',
})
export class FinancesService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiFinancesGet
   */
  static readonly ApiFinancesGetPath = '/api/Finances';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiFinancesGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesGet$Plain$Response(params?: {
  }): Observable<StrictHttpResponse<string>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiFinancesGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<string>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiFinancesGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesGet$Plain(params?: {
  }): Observable<string> {

    return this.apiFinancesGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<string>) => r.body as string)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiFinancesGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<string>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiFinancesGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<string>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiFinancesGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesGet$Json(params?: {
  }): Observable<string> {

    return this.apiFinancesGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<string>) => r.body as string)
    );
  }

  /**
   * Path part for operation apiFinancesNotifyPost
   */
  static readonly ApiFinancesNotifyPostPath = '/api/Finances/notify';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiFinancesNotifyPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesNotifyPost$Response(params?: {
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiFinancesNotifyPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiFinancesNotifyPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesNotifyPost(params?: {
  }): Observable<void> {

    return this.apiFinancesNotifyPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiFinancesUploadEmlPost
   */
  static readonly ApiFinancesUploadEmlPostPath = '/api/Finances/upload-eml';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiFinancesUploadEmlPost()` instead.
   *
   * This method sends `multipart/form-data` and handles request body of type `multipart/form-data`.
   */
  apiFinancesUploadEmlPost$Response(params?: {
    body?: { 'files'?: Array<Blob> }
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiFinancesUploadEmlPostPath, 'post');
    if (params) {
      rb.body(params.body, 'multipart/form-data');
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
   * To access the full response (for headers, for example), `apiFinancesUploadEmlPost$Response()` instead.
   *
   * This method sends `multipart/form-data` and handles request body of type `multipart/form-data`.
   */
  apiFinancesUploadEmlPost(params?: {
    body?: { 'files'?: Array<Blob> }
  }): Observable<void> {

    return this.apiFinancesUploadEmlPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiFinancesAccountsAddPost
   */
  static readonly ApiFinancesAccountsAddPostPath = '/api/Finances/accounts/add';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiFinancesAccountsAddPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiFinancesAccountsAddPost$Response(params?: {
    body?: FinancialAccountDto
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiFinancesAccountsAddPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiFinancesAccountsAddPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiFinancesAccountsAddPost(params?: {
    body?: FinancialAccountDto
  }): Observable<void> {

    return this.apiFinancesAccountsAddPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiFinancesAccountsListGet
   */
  static readonly ApiFinancesAccountsListGetPath = '/api/Finances/accounts/list';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiFinancesAccountsListGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesAccountsListGet$Plain$Response(params?: {
  }): Observable<StrictHttpResponse<Array<FinancialAccountDto>>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiFinancesAccountsListGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<FinancialAccountDto>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiFinancesAccountsListGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesAccountsListGet$Plain(params?: {
  }): Observable<Array<FinancialAccountDto>> {

    return this.apiFinancesAccountsListGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<Array<FinancialAccountDto>>) => r.body as Array<FinancialAccountDto>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiFinancesAccountsListGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesAccountsListGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<Array<FinancialAccountDto>>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiFinancesAccountsListGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<FinancialAccountDto>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiFinancesAccountsListGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesAccountsListGet$Json(params?: {
  }): Observable<Array<FinancialAccountDto>> {

    return this.apiFinancesAccountsListGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<Array<FinancialAccountDto>>) => r.body as Array<FinancialAccountDto>)
    );
  }

  /**
   * Path part for operation apiFinancesAccountsAddTransactionPost
   */
  static readonly ApiFinancesAccountsAddTransactionPostPath = '/api/Finances/accounts/add-transaction';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiFinancesAccountsAddTransactionPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiFinancesAccountsAddTransactionPost$Response(params?: {
    body?: FinancialAccountTransactionDto
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiFinancesAccountsAddTransactionPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiFinancesAccountsAddTransactionPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiFinancesAccountsAddTransactionPost(params?: {
    body?: FinancialAccountTransactionDto
  }): Observable<void> {

    return this.apiFinancesAccountsAddTransactionPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
