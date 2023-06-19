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
export class BackupsService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiBackupsGet
   */
  static readonly ApiBackupsGetPath = '/api/backups';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiBackupsGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiBackupsGet$Plain$Response(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<BackupFilesListDto>> {

    const rb = new RequestBuilder(this.rootUrl, BackupsService.ApiBackupsGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiBackupsGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiBackupsGet$Plain(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<BackupFilesListDto> {

    return this.apiBackupsGet$Plain$Response(params,context).pipe(
      map((r: StrictHttpResponse<BackupFilesListDto>) => r.body as BackupFilesListDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiBackupsGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiBackupsGet$Json$Response(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<StrictHttpResponse<BackupFilesListDto>> {

    const rb = new RequestBuilder(this.rootUrl, BackupsService.ApiBackupsGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiBackupsGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiBackupsGet$Json(params?: {
    first?: number;
    rows?: number;
  },
  context?: HttpContext

): Observable<BackupFilesListDto> {

    return this.apiBackupsGet$Json$Response(params,context).pipe(
      map((r: StrictHttpResponse<BackupFilesListDto>) => r.body as BackupFilesListDto)
    );
  }

  /**
   * Path part for operation apiBackupsPost
   */
  static readonly ApiBackupsPostPath = '/api/backups';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiBackupsPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiBackupsPost$Response(params?: {
    body?: CreateBackup
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, BackupsService.ApiBackupsPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiBackupsPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiBackupsPost(params?: {
    body?: CreateBackup
  },
  context?: HttpContext

): Observable<void> {

    return this.apiBackupsPost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiBackupsDelete
   */
  static readonly ApiBackupsDeletePath = '/api/backups';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiBackupsDelete()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiBackupsDelete$Response(params?: {
    body?: DeleteBackup
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, BackupsService.ApiBackupsDeletePath, 'delete');
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
   * To access the full response (for headers, for example), `apiBackupsDelete$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiBackupsDelete(params?: {
    body?: DeleteBackup
  },
  context?: HttpContext

): Observable<void> {

    return this.apiBackupsDelete$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiBackupsPatch
   */
  static readonly ApiBackupsPatchPath = '/api/backups';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiBackupsPatch()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiBackupsPatch$Response(params?: {
    body?: RenameBackup
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, BackupsService.ApiBackupsPatchPath, 'patch');
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
   * To access the full response (for headers, for example), `apiBackupsPatch$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiBackupsPatch(params?: {
    body?: RenameBackup
  },
  context?: HttpContext

): Observable<void> {

    return this.apiBackupsPatch$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiBackupsRestorePost
   */
  static readonly ApiBackupsRestorePostPath = '/api/backups/restore';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiBackupsRestorePost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiBackupsRestorePost$Response(params?: {
    body?: RestoreBackup
  },
  context?: HttpContext

): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, BackupsService.ApiBackupsRestorePostPath, 'post');
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
   * To access the full response (for headers, for example), `apiBackupsRestorePost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiBackupsRestorePost(params?: {
    body?: RestoreBackup
  },
  context?: HttpContext

): Observable<void> {

    return this.apiBackupsRestorePost$Response(params,context).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
