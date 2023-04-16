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

import { SecretKey } from '../models/secret-key';

@Injectable({
  providedIn: 'root',
})
export class SecretsVaultService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiSecSecretsVaultListGet
   */
  static readonly ApiSecSecretsVaultListGetPath = '/api/sec/SecretsVault/list';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecSecretsVaultListGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecSecretsVaultListGet$Plain$Response(params?: {
  },
  context?: HttpContext

): Observable<StrictHttpResponse<Array<SecretKey>>> {

    const rb = new RequestBuilder(this.rootUrl, SecretsVaultService.ApiSecSecretsVaultListGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<SecretKey>>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiSecSecretsVaultListGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecSecretsVaultListGet$Plain(params?: {
  },
  context?: HttpContext

): Observable<Array<SecretKey>> {

    return this.apiSecSecretsVaultListGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<Array<SecretKey>>) => r.body as Array<SecretKey>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecSecretsVaultListGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecSecretsVaultListGet$Json$Response(params?: {
  },
  context?: HttpContext

): Observable<StrictHttpResponse<Array<SecretKey>>> {

    const rb = new RequestBuilder(this.rootUrl, SecretsVaultService.ApiSecSecretsVaultListGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<SecretKey>>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiSecSecretsVaultListGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecSecretsVaultListGet$Json(params?: {
  },
  context?: HttpContext

): Observable<Array<SecretKey>> {

    return this.apiSecSecretsVaultListGet$Json$Response(params,context).pipe(
      map((r: StrictHttpResponse<Array<SecretKey>>) => r.body as Array<SecretKey>)
    );
  }

  /**
   * Path part for operation apiSecSecretsVaultKeyPost
   */
  static readonly ApiSecSecretsVaultKeyPostPath = '/api/sec/SecretsVault/{key}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecSecretsVaultKeyPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecSecretsVaultKeyPost$Response(params: {
    key: SecretKey;
    value?: string;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, SecretsVaultService.ApiSecSecretsVaultKeyPostPath, 'post');
    if (params) {
      rb.path('key', params.key, {});
      rb.query('value', params.value, {});
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
   * To access the full response (for headers, for example), `apiSecSecretsVaultKeyPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecSecretsVaultKeyPost(params: {
    key: SecretKey;
    value?: string;
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSecSecretsVaultKeyPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecSecretsVaultKeyDelete
   */
  static readonly ApiSecSecretsVaultKeyDeletePath = '/api/sec/SecretsVault/{key}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecSecretsVaultKeyDelete()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecSecretsVaultKeyDelete$Response(params: {
    key: SecretKey;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, SecretsVaultService.ApiSecSecretsVaultKeyDeletePath, 'delete');
    if (params) {
      rb.path('key', params.key, {});
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
   * To access the full response (for headers, for example), `apiSecSecretsVaultKeyDelete$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecSecretsVaultKeyDelete(params: {
    key: SecretKey;
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSecSecretsVaultKeyDelete$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
