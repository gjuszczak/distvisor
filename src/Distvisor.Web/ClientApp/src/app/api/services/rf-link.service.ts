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


@Injectable({
  providedIn: 'root',
})
export class RfLinkService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiRfLinkPost
   */
  static readonly ApiRfLinkPostPath = '/api/rf-link';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiRfLinkPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiRfLinkPost$Response(params: {
    code: string;
    timestamp: number;
    authorization: string;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, RfLinkService.ApiRfLinkPostPath, 'post');
    if (params) {
      rb.query('code', params.code, {});
      rb.query('timestamp', params.timestamp, {});
      rb.header('authorization', params.authorization, {});
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
   * To access the full response (for headers, for example), `apiRfLinkPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiRfLinkPost(params: {
    code: string;
    timestamp: number;
    authorization: string;
  },
  context?: HttpContext

): Observable<void> {

    return this.apiRfLinkPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
