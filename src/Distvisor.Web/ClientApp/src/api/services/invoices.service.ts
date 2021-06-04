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

import { GenerateInvoiceDto } from '../models/generate-invoice-dto';
import { Invoice } from '../models/invoice';

@Injectable({
  providedIn: 'root',
})
export class InvoicesService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiSecInvoicesListGet
   */
  static readonly ApiSecInvoicesListGetPath = '/api/sec/Invoices/list';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecInvoicesListGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecInvoicesListGet$Plain$Response(params?: {
  }): Observable<StrictHttpResponse<Array<Invoice>>> {

    const rb = new RequestBuilder(this.rootUrl, InvoicesService.ApiSecInvoicesListGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<Invoice>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSecInvoicesListGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecInvoicesListGet$Plain(params?: {
  }): Observable<Array<Invoice>> {

    return this.apiSecInvoicesListGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<Array<Invoice>>) => r.body as Array<Invoice>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecInvoicesListGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecInvoicesListGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<Array<Invoice>>> {

    const rb = new RequestBuilder(this.rootUrl, InvoicesService.ApiSecInvoicesListGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<Invoice>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSecInvoicesListGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecInvoicesListGet$Json(params?: {
  }): Observable<Array<Invoice>> {

    return this.apiSecInvoicesListGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<Array<Invoice>>) => r.body as Array<Invoice>)
    );
  }

  /**
   * Path part for operation apiSecInvoicesInvoiceIdGet
   */
  static readonly ApiSecInvoicesInvoiceIdGetPath = '/api/sec/Invoices/{invoiceId}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecInvoicesInvoiceIdGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecInvoicesInvoiceIdGet$Response(params: {
    invoiceId: string;
  }): Observable<StrictHttpResponse<Blob>> {

    const rb = new RequestBuilder(this.rootUrl, InvoicesService.ApiSecInvoicesInvoiceIdGetPath, 'get');
    if (params) {
      rb.path('invoiceId', params.invoiceId, {});
    }

    return this.http.request(rb.build({
      responseType: 'blob',
      accept: 'application/pdf'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Blob>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSecInvoicesInvoiceIdGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecInvoicesInvoiceIdGet(params: {
    invoiceId: string;
  }): Observable<Blob> {

    return this.apiSecInvoicesInvoiceIdGet$Response(params).pipe(
      map((r: StrictHttpResponse<Blob>) => r.body as Blob)
    );
  }

  /**
   * Path part for operation apiSecInvoicesGeneratePost
   */
  static readonly ApiSecInvoicesGeneratePostPath = '/api/sec/Invoices/generate';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecInvoicesGeneratePost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecInvoicesGeneratePost$Response(params?: {
    body?: GenerateInvoiceDto
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, InvoicesService.ApiSecInvoicesGeneratePostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSecInvoicesGeneratePost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecInvoicesGeneratePost(params?: {
    body?: GenerateInvoiceDto
  }): Observable<void> {

    return this.apiSecInvoicesGeneratePost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecInvoicesInvoiceIdSendMailPost
   */
  static readonly ApiSecInvoicesInvoiceIdSendMailPostPath = '/api/sec/Invoices/{invoiceId}/send-mail';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecInvoicesInvoiceIdSendMailPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecInvoicesInvoiceIdSendMailPost$Response(params: {
    invoiceId: string;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, InvoicesService.ApiSecInvoicesInvoiceIdSendMailPostPath, 'post');
    if (params) {
      rb.path('invoiceId', params.invoiceId, {});
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
   * To access the full response (for headers, for example), `apiSecInvoicesInvoiceIdSendMailPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecInvoicesInvoiceIdSendMailPost(params: {
    invoiceId: string;
  }): Observable<void> {

    return this.apiSecInvoicesInvoiceIdSendMailPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
