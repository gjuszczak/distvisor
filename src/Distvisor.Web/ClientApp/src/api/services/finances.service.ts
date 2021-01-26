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

import { AddFinancialAccountDto } from '../models/add-financial-account-dto';
import { AddFinancialAccountTransactionDto } from '../models/add-financial-account-transaction-dto';
import { FinancialAccountDto } from '../models/financial-account-dto';
import { FinancialAccountTransactionDto } from '../models/financial-account-transaction-dto';
import { FinancialSummaryDto } from '../models/financial-summary-dto';

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
   * Path part for operation apiFinancesImportFilesPost
   */
  static readonly ApiFinancesImportFilesPostPath = '/api/Finances/import-files';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiFinancesImportFilesPost()` instead.
   *
   * This method sends `multipart/form-data` and handles request body of type `multipart/form-data`.
   */
  apiFinancesImportFilesPost$Response(params?: {
    body?: { 'files'?: Array<Blob> }
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiFinancesImportFilesPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiFinancesImportFilesPost$Response()` instead.
   *
   * This method sends `multipart/form-data` and handles request body of type `multipart/form-data`.
   */
  apiFinancesImportFilesPost(params?: {
    body?: { 'files'?: Array<Blob> }
  }): Observable<void> {

    return this.apiFinancesImportFilesPost$Response(params).pipe(
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
    body?: AddFinancialAccountDto
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
    body?: AddFinancialAccountDto
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
   * Path part for operation apiFinancesAccountsTransactionsAddPost
   */
  static readonly ApiFinancesAccountsTransactionsAddPostPath = '/api/Finances/accounts/transactions/add';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiFinancesAccountsTransactionsAddPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiFinancesAccountsTransactionsAddPost$Response(params?: {
    body?: AddFinancialAccountTransactionDto
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiFinancesAccountsTransactionsAddPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiFinancesAccountsTransactionsAddPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiFinancesAccountsTransactionsAddPost(params?: {
    body?: AddFinancialAccountTransactionDto
  }): Observable<void> {

    return this.apiFinancesAccountsTransactionsAddPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiFinancesAccountsTransactionsListGet
   */
  static readonly ApiFinancesAccountsTransactionsListGetPath = '/api/Finances/accounts/transactions/list';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiFinancesAccountsTransactionsListGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesAccountsTransactionsListGet$Plain$Response(params?: {
    accountId?: string;
  }): Observable<StrictHttpResponse<Array<FinancialAccountTransactionDto>>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiFinancesAccountsTransactionsListGetPath, 'get');
    if (params) {
      rb.query('accountId', params.accountId, {});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<FinancialAccountTransactionDto>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiFinancesAccountsTransactionsListGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesAccountsTransactionsListGet$Plain(params?: {
    accountId?: string;
  }): Observable<Array<FinancialAccountTransactionDto>> {

    return this.apiFinancesAccountsTransactionsListGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<Array<FinancialAccountTransactionDto>>) => r.body as Array<FinancialAccountTransactionDto>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiFinancesAccountsTransactionsListGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesAccountsTransactionsListGet$Json$Response(params?: {
    accountId?: string;
  }): Observable<StrictHttpResponse<Array<FinancialAccountTransactionDto>>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiFinancesAccountsTransactionsListGetPath, 'get');
    if (params) {
      rb.query('accountId', params.accountId, {});
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<FinancialAccountTransactionDto>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiFinancesAccountsTransactionsListGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesAccountsTransactionsListGet$Json(params?: {
    accountId?: string;
  }): Observable<Array<FinancialAccountTransactionDto>> {

    return this.apiFinancesAccountsTransactionsListGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<Array<FinancialAccountTransactionDto>>) => r.body as Array<FinancialAccountTransactionDto>)
    );
  }

  /**
   * Path part for operation apiFinancesSummaryGet
   */
  static readonly ApiFinancesSummaryGetPath = '/api/Finances/summary';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiFinancesSummaryGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesSummaryGet$Plain$Response(params?: {
  }): Observable<StrictHttpResponse<FinancialSummaryDto>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiFinancesSummaryGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<FinancialSummaryDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiFinancesSummaryGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesSummaryGet$Plain(params?: {
  }): Observable<FinancialSummaryDto> {

    return this.apiFinancesSummaryGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<FinancialSummaryDto>) => r.body as FinancialSummaryDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiFinancesSummaryGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesSummaryGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<FinancialSummaryDto>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiFinancesSummaryGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<FinancialSummaryDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiFinancesSummaryGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiFinancesSummaryGet$Json(params?: {
  }): Observable<FinancialSummaryDto> {

    return this.apiFinancesSummaryGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<FinancialSummaryDto>) => r.body as FinancialSummaryDto)
    );
  }

}
