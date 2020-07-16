/* tslint:disable */
import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';
import { RequestBuilder } from '../request-builder';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';

import { DeployRequestDto } from '../models/deploy-request-dto';
import { DeploymentParamsResponseDto } from '../models/deployment-params-response-dto';
import { RedeployRequestDto } from '../models/redeploy-request-dto';

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
   * Path part for operation apiAdminDeploymentParamsGet
   */
  static readonly ApiAdminDeploymentParamsGetPath = '/api/Admin/deployment-params';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAdminDeploymentParamsGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAdminDeploymentParamsGet$Plain$Response(params?: {

  }): Observable<StrictHttpResponse<DeploymentParamsResponseDto>> {

    const rb = new RequestBuilder(this.rootUrl, AdminService.ApiAdminDeploymentParamsGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<DeploymentParamsResponseDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiAdminDeploymentParamsGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAdminDeploymentParamsGet$Plain(params?: {

  }): Observable<DeploymentParamsResponseDto> {

    return this.apiAdminDeploymentParamsGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<DeploymentParamsResponseDto>) => r.body as DeploymentParamsResponseDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAdminDeploymentParamsGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAdminDeploymentParamsGet$Json$Response(params?: {

  }): Observable<StrictHttpResponse<DeploymentParamsResponseDto>> {

    const rb = new RequestBuilder(this.rootUrl, AdminService.ApiAdminDeploymentParamsGetPath, 'get');
    if (params) {


    }
    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<DeploymentParamsResponseDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiAdminDeploymentParamsGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiAdminDeploymentParamsGet$Json(params?: {

  }): Observable<DeploymentParamsResponseDto> {

    return this.apiAdminDeploymentParamsGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<DeploymentParamsResponseDto>) => r.body as DeploymentParamsResponseDto)
    );
  }

  /**
   * Path part for operation apiAdminDeployPost
   */
  static readonly ApiAdminDeployPostPath = '/api/Admin/deploy';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAdminDeployPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiAdminDeployPost$Response(params?: {
      body?: DeployRequestDto
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AdminService.ApiAdminDeployPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiAdminDeployPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiAdminDeployPost(params?: {
      body?: DeployRequestDto
  }): Observable<void> {

    return this.apiAdminDeployPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiAdminRedeployPost
   */
  static readonly ApiAdminRedeployPostPath = '/api/Admin/redeploy';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiAdminRedeployPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiAdminRedeployPost$Response(params?: {
      body?: RedeployRequestDto
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AdminService.ApiAdminRedeployPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiAdminRedeployPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiAdminRedeployPost(params?: {
      body?: RedeployRequestDto
  }): Observable<void> {

    return this.apiAdminRedeployPost$Response(params).pipe(
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
