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
   * Path part for operation apiSecFinancesImportFilesPost
   */
  static readonly ApiSecFinancesImportFilesPostPath = '/api/sec/Finances/import-files';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecFinancesImportFilesPost()` instead.
   *
   * This method sends `multipart/form-data` and handles request body of type `multipart/form-data`.
   */
  apiSecFinancesImportFilesPost$Response(params?: {
    body?: { 'files'?: Array<Blob> }
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiSecFinancesImportFilesPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSecFinancesImportFilesPost$Response()` instead.
   *
   * This method sends `multipart/form-data` and handles request body of type `multipart/form-data`.
   */
  apiSecFinancesImportFilesPost(params?: {
    body?: { 'files'?: Array<Blob> }
  }): Observable<void> {

    return this.apiSecFinancesImportFilesPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecFinancesAccountsAddPost
   */
  static readonly ApiSecFinancesAccountsAddPostPath = '/api/sec/Finances/accounts/add';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecFinancesAccountsAddPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecFinancesAccountsAddPost$Response(params?: {
    body?: AddFinancialAccountDto
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiSecFinancesAccountsAddPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSecFinancesAccountsAddPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecFinancesAccountsAddPost(params?: {
    body?: AddFinancialAccountDto
  }): Observable<void> {

    return this.apiSecFinancesAccountsAddPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecFinancesAccountsListGet
   */
  static readonly ApiSecFinancesAccountsListGetPath = '/api/sec/Finances/accounts/list';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecFinancesAccountsListGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecFinancesAccountsListGet$Plain$Response(params?: {
  }): Observable<StrictHttpResponse<Array<FinancialAccountDto>>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiSecFinancesAccountsListGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiSecFinancesAccountsListGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecFinancesAccountsListGet$Plain(params?: {
  }): Observable<Array<FinancialAccountDto>> {

    return this.apiSecFinancesAccountsListGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<Array<FinancialAccountDto>>) => r.body as Array<FinancialAccountDto>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecFinancesAccountsListGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecFinancesAccountsListGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<Array<FinancialAccountDto>>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiSecFinancesAccountsListGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiSecFinancesAccountsListGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecFinancesAccountsListGet$Json(params?: {
  }): Observable<Array<FinancialAccountDto>> {

    return this.apiSecFinancesAccountsListGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<Array<FinancialAccountDto>>) => r.body as Array<FinancialAccountDto>)
    );
  }

  /**
   * Path part for operation apiSecFinancesAccountsTransactionsAddPost
   */
  static readonly ApiSecFinancesAccountsTransactionsAddPostPath = '/api/sec/Finances/accounts/transactions/add';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecFinancesAccountsTransactionsAddPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecFinancesAccountsTransactionsAddPost$Response(params?: {
    body?: AddFinancialAccountTransactionDto
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiSecFinancesAccountsTransactionsAddPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSecFinancesAccountsTransactionsAddPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecFinancesAccountsTransactionsAddPost(params?: {
    body?: AddFinancialAccountTransactionDto
  }): Observable<void> {

    return this.apiSecFinancesAccountsTransactionsAddPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecFinancesAccountsTransactionsListGet
   */
  static readonly ApiSecFinancesAccountsTransactionsListGetPath = '/api/sec/Finances/accounts/transactions/list';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecFinancesAccountsTransactionsListGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecFinancesAccountsTransactionsListGet$Plain$Response(params?: {
    accountId?: string;
  }): Observable<StrictHttpResponse<Array<FinancialAccountTransactionDto>>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiSecFinancesAccountsTransactionsListGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiSecFinancesAccountsTransactionsListGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecFinancesAccountsTransactionsListGet$Plain(params?: {
    accountId?: string;
  }): Observable<Array<FinancialAccountTransactionDto>> {

    return this.apiSecFinancesAccountsTransactionsListGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<Array<FinancialAccountTransactionDto>>) => r.body as Array<FinancialAccountTransactionDto>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecFinancesAccountsTransactionsListGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecFinancesAccountsTransactionsListGet$Json$Response(params?: {
    accountId?: string;
  }): Observable<StrictHttpResponse<Array<FinancialAccountTransactionDto>>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiSecFinancesAccountsTransactionsListGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiSecFinancesAccountsTransactionsListGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecFinancesAccountsTransactionsListGet$Json(params?: {
    accountId?: string;
  }): Observable<Array<FinancialAccountTransactionDto>> {

    return this.apiSecFinancesAccountsTransactionsListGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<Array<FinancialAccountTransactionDto>>) => r.body as Array<FinancialAccountTransactionDto>)
    );
  }

  /**
   * Path part for operation apiSecFinancesSummaryGet
   */
  static readonly ApiSecFinancesSummaryGetPath = '/api/sec/Finances/summary';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecFinancesSummaryGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecFinancesSummaryGet$Plain$Response(params?: {
  }): Observable<StrictHttpResponse<FinancialSummaryDto>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiSecFinancesSummaryGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiSecFinancesSummaryGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecFinancesSummaryGet$Plain(params?: {
  }): Observable<FinancialSummaryDto> {

    return this.apiSecFinancesSummaryGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<FinancialSummaryDto>) => r.body as FinancialSummaryDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecFinancesSummaryGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecFinancesSummaryGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<FinancialSummaryDto>> {

    const rb = new RequestBuilder(this.rootUrl, FinancesService.ApiSecFinancesSummaryGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiSecFinancesSummaryGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecFinancesSummaryGet$Json(params?: {
  }): Observable<FinancialSummaryDto> {

    return this.apiSecFinancesSummaryGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<FinancialSummaryDto>) => r.body as FinancialSummaryDto)
    );
  }

}
