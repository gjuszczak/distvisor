/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

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
   * Path part for operation apiInvoicesListGet
   */
  static readonly ApiInvoicesListGetPath = '/api/Invoices/list';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiInvoicesListGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiInvoicesListGet$Plain$Response(params?: {

  }): Observable<StrictHttpResponse<null | Array<Invoice>>> {

    const rb = new RequestBuilder(this.rootUrl, InvoicesService.ApiInvoicesListGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<null | Array<Invoice>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiInvoicesListGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiInvoicesListGet$Plain(params?: {

  }): Observable<null | Array<Invoice>> {

    return this.apiInvoicesListGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<null | Array<Invoice>>) => r.body as null | Array<Invoice>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiInvoicesListGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiInvoicesListGet$Json$Response(params?: {

  }): Observable<StrictHttpResponse<null | Array<Invoice>>> {

    const rb = new RequestBuilder(this.rootUrl, InvoicesService.ApiInvoicesListGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'application/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<null | Array<Invoice>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiInvoicesListGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiInvoicesListGet$Json(params?: {

  }): Observable<null | Array<Invoice>> {

    return this.apiInvoicesListGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<null | Array<Invoice>>) => r.body as null | Array<Invoice>)
    );
  }

  /**
   * Path part for operation apiInvoicesInvoiceIdGet
   */
  static readonly ApiInvoicesInvoiceIdGetPath = '/api/Invoices/{invoiceId}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiInvoicesInvoiceIdGet()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiInvoicesInvoiceIdGet$Response(params: {
    invoiceId: null | string;

  }): Observable<StrictHttpResponse<Blob>> {

    const rb = new RequestBuilder(this.rootUrl, InvoicesService.ApiInvoicesInvoiceIdGetPath, 'get');
    if (params) {

      rb.path('invoiceId', params.invoiceId);

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
   * To access the full response (for headers, for example), `apiInvoicesInvoiceIdGet$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiInvoicesInvoiceIdGet(params: {
    invoiceId: null | string;

  }): Observable<Blob> {

    return this.apiInvoicesInvoiceIdGet$Response(params).pipe(
      map((r: StrictHttpResponse<Blob>) => r.body as Blob)
    );
  }

  /**
   * Path part for operation apiInvoicesGeneratePost
   */
  static readonly ApiInvoicesGeneratePostPath = '/api/Invoices/generate';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiInvoicesGeneratePost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiInvoicesGeneratePost$Response(params?: {
    templateInvoiceId?: null | string;

  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, InvoicesService.ApiInvoicesGeneratePostPath, 'post');
    if (params) {

      rb.query('templateInvoiceId', params.templateInvoiceId);

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
   * To access the full response (for headers, for example), `apiInvoicesGeneratePost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiInvoicesGeneratePost(params?: {
    templateInvoiceId?: null | string;

  }): Observable<void> {

    return this.apiInvoicesGeneratePost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiInvoicesInvoiceIdSendMailPost
   */
  static readonly ApiInvoicesInvoiceIdSendMailPostPath = '/api/Invoices/{invoiceId}/send-mail';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiInvoicesInvoiceIdSendMailPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiInvoicesInvoiceIdSendMailPost$Response(params: {
    invoiceId: null | string;

  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, InvoicesService.ApiInvoicesInvoiceIdSendMailPostPath, 'post');
    if (params) {

      rb.path('invoiceId', params.invoiceId);

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
   * To access the full response (for headers, for example), `apiInvoicesInvoiceIdSendMailPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiInvoicesInvoiceIdSendMailPost(params: {
    invoiceId: null | string;

  }): Observable<void> {

    return this.apiInvoicesInvoiceIdSendMailPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
