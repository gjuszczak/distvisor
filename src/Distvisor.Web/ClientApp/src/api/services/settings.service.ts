/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

import { UpdateParamsResponseDto } from '../models/update-params-response-dto';

@Injectable({
  providedIn: 'root',
})
export class SettingsService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiSettingsUpdateParamsGet
   */
  static readonly ApiSettingsUpdateParamsGetPath = '/api/Settings/update-params';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSettingsUpdateParamsGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSettingsUpdateParamsGet$Plain$Response(params?: {

  }): Observable<StrictHttpResponse<UpdateParamsResponseDto>> {

    const rb = new RequestBuilder(this.rootUrl, SettingsService.ApiSettingsUpdateParamsGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<UpdateParamsResponseDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSettingsUpdateParamsGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSettingsUpdateParamsGet$Plain(params?: {

  }): Observable<UpdateParamsResponseDto> {

    return this.apiSettingsUpdateParamsGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<UpdateParamsResponseDto>) => r.body as UpdateParamsResponseDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSettingsUpdateParamsGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSettingsUpdateParamsGet$Json$Response(params?: {

  }): Observable<StrictHttpResponse<UpdateParamsResponseDto>> {

    const rb = new RequestBuilder(this.rootUrl, SettingsService.ApiSettingsUpdateParamsGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'application/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<UpdateParamsResponseDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSettingsUpdateParamsGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSettingsUpdateParamsGet$Json(params?: {

  }): Observable<UpdateParamsResponseDto> {

    return this.apiSettingsUpdateParamsGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<UpdateParamsResponseDto>) => r.body as UpdateParamsResponseDto)
    );
  }

  /**
   * Path part for operation apiSettingsUpdatePost
   */
  static readonly ApiSettingsUpdatePostPath = '/api/Settings/update';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSettingsUpdatePost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSettingsUpdatePost$Response(params?: {
    tag?: null | string;

  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, SettingsService.ApiSettingsUpdatePostPath, 'post');
    if (params) {

      rb.query('tag', params.tag);

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
   * To access the full response (for headers, for example), `apiSettingsUpdatePost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSettingsUpdatePost(params?: {
    tag?: null | string;

  }): Observable<void> {

    return this.apiSettingsUpdatePost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
