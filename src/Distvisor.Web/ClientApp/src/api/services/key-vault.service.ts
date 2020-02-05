/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

import { KeyType } from '../models/key-type';
import { Object } from '../models/object';

@Injectable({
  providedIn: 'root',
})
export class KeyVaultService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiKeyVaultListGet
   */
  static readonly ApiKeyVaultListGetPath = '/api/KeyVault/list';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiKeyVaultListGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiKeyVaultListGet$Plain$Response(params?: {

  }): Observable<StrictHttpResponse<null | Array<KeyType>>> {

    const rb = new RequestBuilder(this.rootUrl, KeyVaultService.ApiKeyVaultListGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<null | Array<KeyType>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiKeyVaultListGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiKeyVaultListGet$Plain(params?: {

  }): Observable<null | Array<KeyType>> {

    return this.apiKeyVaultListGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<null | Array<KeyType>>) => r.body as null | Array<KeyType>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiKeyVaultListGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiKeyVaultListGet$Json$Response(params?: {

  }): Observable<StrictHttpResponse<null | Array<KeyType>>> {

    const rb = new RequestBuilder(this.rootUrl, KeyVaultService.ApiKeyVaultListGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'application/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<null | Array<KeyType>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiKeyVaultListGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiKeyVaultListGet$Json(params?: {

  }): Observable<null | Array<KeyType>> {

    return this.apiKeyVaultListGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<null | Array<KeyType>>) => r.body as null | Array<KeyType>)
    );
  }

  /**
   * Path part for operation apiKeyVaultKeyTypePost
   */
  static readonly ApiKeyVaultKeyTypePostPath = '/api/KeyVault/{keyType}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiKeyVaultKeyTypePost$Json()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  apiKeyVaultKeyTypePost$Json$Response(params: {
    keyType: KeyType;

    body?: Object
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, KeyVaultService.ApiKeyVaultKeyTypePostPath, 'post');
    if (params) {

      rb.path('keyType', params.keyType);

      rb.body(params.body, 'application/json');
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
   * To access the full response (for headers, for example), `apiKeyVaultKeyTypePost$Json$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  apiKeyVaultKeyTypePost$Json(params: {
    keyType: KeyType;

    body?: Object
  }): Observable<void> {

    return this.apiKeyVaultKeyTypePost$Json$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiKeyVaultKeyTypeDelete
   */
  static readonly ApiKeyVaultKeyTypeDeletePath = '/api/KeyVault/{keyType}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiKeyVaultKeyTypeDelete()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiKeyVaultKeyTypeDelete$Response(params: {
    keyType: KeyType;

  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, KeyVaultService.ApiKeyVaultKeyTypeDeletePath, 'delete');
    if (params) {

      rb.path('keyType', params.keyType);

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
   * To access the full response (for headers, for example), `apiKeyVaultKeyTypeDelete$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiKeyVaultKeyTypeDelete(params: {
    keyType: KeyType;

  }): Observable<void> {

    return this.apiKeyVaultKeyTypeDelete$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
