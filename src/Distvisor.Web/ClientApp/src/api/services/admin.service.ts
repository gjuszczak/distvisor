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
import { UpdateRequestDto } from '../models/update-request-dto';

@Injectable({
  providedIn: 'root',
})
export class AdminService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiAdminUpdateParamsGet
   */
  static readonly ApiAdminUpdateParamsGetPath = '/api/Admin/update-params';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAdminUpdateParamsGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAdminUpdateParamsGet$Plain$Response(params?: {

  }): Observable<StrictHttpResponse<UpdateParamsResponseDto>> {

    const rb = new RequestBuilder(this.rootUrl, AdminService.ApiAdminUpdateParamsGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiAdminUpdateParamsGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAdminUpdateParamsGet$Plain(params?: {

  }): Observable<UpdateParamsResponseDto> {

    return this.apiAdminUpdateParamsGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<UpdateParamsResponseDto>) => r.body as UpdateParamsResponseDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAdminUpdateParamsGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAdminUpdateParamsGet$Json$Response(params?: {

  }): Observable<StrictHttpResponse<UpdateParamsResponseDto>> {

    const rb = new RequestBuilder(this.rootUrl, AdminService.ApiAdminUpdateParamsGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiAdminUpdateParamsGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAdminUpdateParamsGet$Json(params?: {

  }): Observable<UpdateParamsResponseDto> {

    return this.apiAdminUpdateParamsGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<UpdateParamsResponseDto>) => r.body as UpdateParamsResponseDto)
    );
  }

  /**
   * Path part for operation apiAdminUpdatePost
   */
  static readonly ApiAdminUpdatePostPath = '/api/Admin/update';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAdminUpdatePost$Json()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  apiAdminUpdatePost$Json$Response(params?: {

    body?: UpdateRequestDto
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AdminService.ApiAdminUpdatePostPath, 'post');
    if (params) {


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
   * To access the full response (for headers, for example), `apiAdminUpdatePost$Json$Response()` instead.
   *
   * This method sends `application/json` and handles request body of type `application/json`.
   */
  apiAdminUpdatePost$Json(params?: {

    body?: UpdateRequestDto
  }): Observable<void> {

    return this.apiAdminUpdatePost$Json$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiAdminBackupPost
   */
  static readonly ApiAdminBackupPostPath = '/api/Admin/backup';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAdminBackupPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAdminBackupPost$Response(params?: {

  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AdminService.ApiAdminBackupPostPath, 'post');
    if (params) {


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
   * To access the full response (for headers, for example), `apiAdminBackupPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAdminBackupPost(params?: {

  }): Observable<void> {

    return this.apiAdminBackupPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
