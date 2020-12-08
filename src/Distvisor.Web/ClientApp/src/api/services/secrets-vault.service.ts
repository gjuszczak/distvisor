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
   * Path part for operation apiSecretsVaultListGet
   */
  static readonly ApiSecretsVaultListGetPath = '/api/SecretsVault/list';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecretsVaultListGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecretsVaultListGet$Plain$Response(params?: {
  }): Observable<StrictHttpResponse<Array<SecretKey>>> {

    const rb = new RequestBuilder(this.rootUrl, SecretsVaultService.ApiSecretsVaultListGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<SecretKey>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSecretsVaultListGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecretsVaultListGet$Plain(params?: {
  }): Observable<Array<SecretKey>> {

    return this.apiSecretsVaultListGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<Array<SecretKey>>) => r.body as Array<SecretKey>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecretsVaultListGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecretsVaultListGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<Array<SecretKey>>> {

    const rb = new RequestBuilder(this.rootUrl, SecretsVaultService.ApiSecretsVaultListGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<SecretKey>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSecretsVaultListGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecretsVaultListGet$Json(params?: {
  }): Observable<Array<SecretKey>> {

    return this.apiSecretsVaultListGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<Array<SecretKey>>) => r.body as Array<SecretKey>)
    );
  }

  /**
   * Path part for operation apiSecretsVaultKeyPost
   */
  static readonly ApiSecretsVaultKeyPostPath = '/api/SecretsVault/{key}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecretsVaultKeyPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecretsVaultKeyPost$Response(params: {
    key: SecretKey;
    value?: string;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, SecretsVaultService.ApiSecretsVaultKeyPostPath, 'post');
    if (params) {
      rb.path('key', params.key, {});
      rb.query('value', params.value, {});
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
   * To access the full response (for headers, for example), `apiSecretsVaultKeyPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecretsVaultKeyPost(params: {
    key: SecretKey;
    value?: string;
  }): Observable<void> {

    return this.apiSecretsVaultKeyPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecretsVaultKeyDelete
   */
  static readonly ApiSecretsVaultKeyDeletePath = '/api/SecretsVault/{key}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecretsVaultKeyDelete()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecretsVaultKeyDelete$Response(params: {
    key: SecretKey;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, SecretsVaultService.ApiSecretsVaultKeyDeletePath, 'delete');
    if (params) {
      rb.path('key', params.key, {});
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
   * To access the full response (for headers, for example), `apiSecretsVaultKeyDelete$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecretsVaultKeyDelete(params: {
    key: SecretKey;
  }): Observable<void> {

    return this.apiSecretsVaultKeyDelete$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
