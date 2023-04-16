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

import { BackupFilesListDto } from '../models/backup-files-list-dto';
import { CreateBackup } from '../models/create-backup';
import { DeleteBackup } from '../models/delete-backup';
import { RenameBackup } from '../models/rename-backup';
import { RestoreBackup } from '../models/restore-backup';

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
   * Path part for operation apiSAdminBackupsGet
   */
  static readonly ApiSAdminBackupsGetPath = '/api/s/Admin/backups';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSAdminBackupsGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSAdminBackupsGet$Plain$Response(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<BackupFilesListDto>> {

    const rb = new RequestBuilder(this.rootUrl, AdminService.ApiSAdminBackupsGetPath, 'get');
    if (params) {
      rb.query('first', params.first, {});
      rb.query('rows', params.rows, {});
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<BackupFilesListDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiSAdminBackupsGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSAdminBackupsGet$Plain(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<BackupFilesListDto> {

    return this.apiSAdminBackupsGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<BackupFilesListDto>) => r.body as BackupFilesListDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSAdminBackupsGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSAdminBackupsGet$Json$Response(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<BackupFilesListDto>> {

    const rb = new RequestBuilder(this.rootUrl, AdminService.ApiSAdminBackupsGetPath, 'get');
    if (params) {
      rb.query('first', params.first, {});
      rb.query('rows', params.rows, {});
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<BackupFilesListDto>;
      })
    );
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `apiSAdminBackupsGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSAdminBackupsGet$Json(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<BackupFilesListDto> {

    return this.apiSAdminBackupsGet$Json$Response(params,context).pipe(
      map((r: StrictHttpResponse<BackupFilesListDto>) => r.body as BackupFilesListDto)
    );
  }

  /**
   * Path part for operation apiSAdminBackupsPost
   */
  static readonly ApiSAdminBackupsPostPath = '/api/s/Admin/backups';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSAdminBackupsPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSAdminBackupsPost$Response(params?: {
    body?: CreateBackup
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AdminService.ApiSAdminBackupsPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSAdminBackupsPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSAdminBackupsPost(params?: {
    body?: CreateBackup
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSAdminBackupsPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSAdminBackupsDelete
   */
  static readonly ApiSAdminBackupsDeletePath = '/api/s/Admin/backups';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSAdminBackupsDelete()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSAdminBackupsDelete$Response(params?: {
    body?: DeleteBackup
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AdminService.ApiSAdminBackupsDeletePath, 'delete');
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
   * To access the full response (for headers, for example), `apiSAdminBackupsDelete$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSAdminBackupsDelete(params?: {
    body?: DeleteBackup
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSAdminBackupsDelete$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSAdminBackupsPatch
   */
  static readonly ApiSAdminBackupsPatchPath = '/api/s/Admin/backups';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSAdminBackupsPatch()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSAdminBackupsPatch$Response(params?: {
    body?: RenameBackup
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AdminService.ApiSAdminBackupsPatchPath, 'patch');
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
   * To access the full response (for headers, for example), `apiSAdminBackupsPatch$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSAdminBackupsPatch(params?: {
    body?: RenameBackup
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSAdminBackupsPatch$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSAdminBackupsRestorePost
   */
  static readonly ApiSAdminBackupsRestorePostPath = '/api/s/Admin/backups/restore';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSAdminBackupsRestorePost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSAdminBackupsRestorePost$Response(params?: {
    body?: RestoreBackup
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, AdminService.ApiSAdminBackupsRestorePostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSAdminBackupsRestorePost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSAdminBackupsRestorePost(params?: {
    body?: RestoreBackup
  },
  context?: HttpContext

): Observable<void> {

    return this.apiSAdminBackupsRestorePost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
