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

import { BackupFileInfoDto } from '../models/backup-file-info-dto';
import { DeployRequestDto } from '../models/deploy-request-dto';
import { DeploymentParamsResponseDto } from '../models/deployment-params-response-dto';
import { RedeployRequestDto } from '../models/redeploy-request-dto';

@Injectable({
  providedIn: 'root',
})
export class AdminDeprService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiSecAdminDeploymentParamsGet
   */
  static readonly ApiSecAdminDeploymentParamsGetPath = '/api/sec/Admin/deployment-params';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecAdminDeploymentParamsGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAdminDeploymentParamsGet$Plain$Response(params?: {
  },
  context?: HttpContext

): Observable<StrictHttpResponse<DeploymentParamsResponseDto>> {

    const rb = new RequestBuilder(this.rootUrl, AdminDeprService.ApiSecAdminDeploymentParamsGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<DeploymentParamsResponseDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiSecAdminDeploymentParamsGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAdminDeploymentParamsGet$Plain(params?: {
  },
  context?: HttpContext

): Observable<DeploymentParamsResponseDto> {

    return this.apiSecAdminDeploymentParamsGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<DeploymentParamsResponseDto>) => r.body as DeploymentParamsResponseDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecAdminDeploymentParamsGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAdminDeploymentParamsGet$Json$Response(params?: {
  },
  context?: HttpContext

): Observable<StrictHttpResponse<DeploymentParamsResponseDto>> {

    const rb = new RequestBuilder(this.rootUrl, AdminDeprService.ApiSecAdminDeploymentParamsGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<DeploymentParamsResponseDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiSecAdminDeploymentParamsGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAdminDeploymentParamsGet$Json(params?: {
  },
  context?: HttpContext

): Observable<DeploymentParamsResponseDto> {

    return this.apiSecAdminDeploymentParamsGet$Json$Response(params,context).pipe(
      map((r: StrictHttpResponse<DeploymentParamsResponseDto>) => r.body as DeploymentParamsResponseDto)
    );
  }

  /**
   * Path part for operation apiSecAdminDeployPost
   */
  static readonly ApiSecAdminDeployPostPath = '/api/sec/Admin/deploy';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecAdminDeployPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecAdminDeployPost$Response(params?: {
    body?: DeployRequestDto
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AdminDeprService.ApiSecAdminDeployPostPath, 'post');
    if (params) {
      rb.body(params.body, 'application/*+json');
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
   * To access the full response (for headers, for example), `apiSecAdminDeployPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecAdminDeployPost(params?: {
    body?: DeployRequestDto
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSecAdminDeployPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecAdminRedeployPost
   */
  static readonly ApiSecAdminRedeployPostPath = '/api/sec/Admin/redeploy';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecAdminRedeployPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecAdminRedeployPost$Response(params?: {
    body?: RedeployRequestDto
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AdminDeprService.ApiSecAdminRedeployPostPath, 'post');
    if (params) {
      rb.body(params.body, 'application/*+json');
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
   * To access the full response (for headers, for example), `apiSecAdminRedeployPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecAdminRedeployPost(params?: {
    body?: RedeployRequestDto
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSecAdminRedeployPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecAdminListBackupsGet
   */
  static readonly ApiSecAdminListBackupsGetPath = '/api/sec/Admin/list-backups';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecAdminListBackupsGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAdminListBackupsGet$Plain$Response(params?: {
  },
  context?: HttpContext

): Observable<StrictHttpResponse<Array<BackupFileInfoDto>>> {

    const rb = new RequestBuilder(this.rootUrl, AdminDeprService.ApiSecAdminListBackupsGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<BackupFileInfoDto>>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiSecAdminListBackupsGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAdminListBackupsGet$Plain(params?: {
  },
  context?: HttpContext

): Observable<Array<BackupFileInfoDto>> {

    return this.apiSecAdminListBackupsGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<Array<BackupFileInfoDto>>) => r.body as Array<BackupFileInfoDto>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecAdminListBackupsGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAdminListBackupsGet$Json$Response(params?: {
  },
  context?: HttpContext

): Observable<StrictHttpResponse<Array<BackupFileInfoDto>>> {

    const rb = new RequestBuilder(this.rootUrl, AdminDeprService.ApiSecAdminListBackupsGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<BackupFileInfoDto>>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiSecAdminListBackupsGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAdminListBackupsGet$Json(params?: {
  },
  context?: HttpContext

): Observable<Array<BackupFileInfoDto>> {

    return this.apiSecAdminListBackupsGet$Json$Response(params,context).pipe(
      map((r: StrictHttpResponse<Array<BackupFileInfoDto>>) => r.body as Array<BackupFileInfoDto>)
    );
  }

  /**
   * Path part for operation apiSecAdminCreateBackupPost
   */
  static readonly ApiSecAdminCreateBackupPostPath = '/api/sec/Admin/create-backup';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecAdminCreateBackupPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAdminCreateBackupPost$Response(params?: {
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AdminDeprService.ApiSecAdminCreateBackupPostPath, 'post');
    if (params) {
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
   * To access the full response (for headers, for example), `apiSecAdminCreateBackupPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAdminCreateBackupPost(params?: {
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSecAdminCreateBackupPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecAdminRestoreBackupPost
   */
  static readonly ApiSecAdminRestoreBackupPostPath = '/api/sec/Admin/restore-backup';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecAdminRestoreBackupPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecAdminRestoreBackupPost$Response(params?: {
    body?: BackupFileInfoDto
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AdminDeprService.ApiSecAdminRestoreBackupPostPath, 'post');
    if (params) {
      rb.body(params.body, 'application/*+json');
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
   * To access the full response (for headers, for example), `apiSecAdminRestoreBackupPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecAdminRestoreBackupPost(params?: {
    body?: BackupFileInfoDto
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSecAdminRestoreBackupPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecAdminDeleteBackupPost
   */
  static readonly ApiSecAdminDeleteBackupPostPath = '/api/sec/Admin/delete-backup';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecAdminDeleteBackupPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecAdminDeleteBackupPost$Response(params?: {
    body?: BackupFileInfoDto
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AdminDeprService.ApiSecAdminDeleteBackupPostPath, 'post');
    if (params) {
      rb.body(params.body, 'application/*+json');
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
   * To access the full response (for headers, for example), `apiSecAdminDeleteBackupPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecAdminDeleteBackupPost(params?: {
    body?: BackupFileInfoDto
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSecAdminDeleteBackupPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecAdminReplayEventsPost
   */
  static readonly ApiSecAdminReplayEventsPostPath = '/api/sec/Admin/replay-events';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecAdminReplayEventsPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAdminReplayEventsPost$Response(params?: {
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AdminDeprService.ApiSecAdminReplayEventsPostPath, 'post');
    if (params) {
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
   * To access the full response (for headers, for example), `apiSecAdminReplayEventsPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecAdminReplayEventsPost(params?: {
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSecAdminReplayEventsPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
