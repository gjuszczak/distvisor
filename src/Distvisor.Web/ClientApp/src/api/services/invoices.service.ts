/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';


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

  }): Observable<StrictHttpResponse<null | Array<string>>> {

    const rb = new RequestBuilder(this.rootUrl, InvoicesService.ApiInvoicesListGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<null | Array<string>>;
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

  }): Observable<null | Array<string>> {

    return this.apiInvoicesListGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<null | Array<string>>) => r.body as null | Array<string>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiInvoicesListGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiInvoicesListGet$Json$Response(params?: {

  }): Observable<StrictHttpResponse<null | Array<string>>> {

    const rb = new RequestBuilder(this.rootUrl, InvoicesService.ApiInvoicesListGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'application/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<null | Array<string>>;
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

  }): Observable<null | Array<string>> {

    return this.apiInvoicesListGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<null | Array<string>>) => r.body as null | Array<string>)
    );
  }

}
