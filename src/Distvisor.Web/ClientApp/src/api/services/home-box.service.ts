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

import { AddHomeBoxTriggerDto } from '../models/add-home-box-trigger-dto';
import { DeviceDto } from '../models/device-dto';
import { HomeBoxTriggerDto } from '../models/home-box-trigger-dto';

@Injectable({
  providedIn: 'root',
})
export class HomeBoxService extends BaseService {
  constructor(
    config: ApiConfiguration,
    http: HttpClient
  ) {
    super(config, http);
  }

  /**
   * Path part for operation apiSecHomeBoxDevicesGet
   */
  static readonly ApiSecHomeBoxDevicesGetPath = '/api/sec/home-box/devices';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxDevicesGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxDevicesGet$Plain$Response(params?: {
  }): Observable<StrictHttpResponse<Array<DeviceDto>>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxDevicesGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<DeviceDto>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSecHomeBoxDevicesGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxDevicesGet$Plain(params?: {
  }): Observable<Array<DeviceDto>> {

    return this.apiSecHomeBoxDevicesGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<Array<DeviceDto>>) => r.body as Array<DeviceDto>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxDevicesGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxDevicesGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<Array<DeviceDto>>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxDevicesGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<DeviceDto>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSecHomeBoxDevicesGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxDevicesGet$Json(params?: {
  }): Observable<Array<DeviceDto>> {

    return this.apiSecHomeBoxDevicesGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<Array<DeviceDto>>) => r.body as Array<DeviceDto>)
    );
  }

  /**
   * Path part for operation apiSecHomeBoxDevicesIdentifierTurnOnPost
   */
  static readonly ApiSecHomeBoxDevicesIdentifierTurnOnPostPath = '/api/sec/home-box/devices/{identifier}/turnOn';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxDevicesIdentifierTurnOnPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxDevicesIdentifierTurnOnPost$Response(params: {
    identifier: string;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxDevicesIdentifierTurnOnPostPath, 'post');
    if (params) {
      rb.path('identifier', params.identifier, {});
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
   * To access the full response (for headers, for example), `apiSecHomeBoxDevicesIdentifierTurnOnPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxDevicesIdentifierTurnOnPost(params: {
    identifier: string;
  }): Observable<void> {

    return this.apiSecHomeBoxDevicesIdentifierTurnOnPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecHomeBoxDevicesIdentifierTurnOffPost
   */
  static readonly ApiSecHomeBoxDevicesIdentifierTurnOffPostPath = '/api/sec/home-box/devices/{identifier}/turnOff';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxDevicesIdentifierTurnOffPost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxDevicesIdentifierTurnOffPost$Response(params: {
    identifier: string;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxDevicesIdentifierTurnOffPostPath, 'post');
    if (params) {
      rb.path('identifier', params.identifier, {});
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
   * To access the full response (for headers, for example), `apiSecHomeBoxDevicesIdentifierTurnOffPost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxDevicesIdentifierTurnOffPost(params: {
    identifier: string;
  }): Observable<void> {

    return this.apiSecHomeBoxDevicesIdentifierTurnOffPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecHomeBoxTriggersListGet
   */
  static readonly ApiSecHomeBoxTriggersListGetPath = '/api/sec/home-box/triggers/list';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxTriggersListGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersListGet$Plain$Response(params?: {
  }): Observable<StrictHttpResponse<Array<HomeBoxTriggerDto>>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxTriggersListGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<HomeBoxTriggerDto>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSecHomeBoxTriggersListGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersListGet$Plain(params?: {
  }): Observable<Array<HomeBoxTriggerDto>> {

    return this.apiSecHomeBoxTriggersListGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<Array<HomeBoxTriggerDto>>) => r.body as Array<HomeBoxTriggerDto>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxTriggersListGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersListGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<Array<HomeBoxTriggerDto>>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxTriggersListGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<HomeBoxTriggerDto>>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSecHomeBoxTriggersListGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersListGet$Json(params?: {
  }): Observable<Array<HomeBoxTriggerDto>> {

    return this.apiSecHomeBoxTriggersListGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<Array<HomeBoxTriggerDto>>) => r.body as Array<HomeBoxTriggerDto>)
    );
  }

  /**
   * Path part for operation apiSecHomeBoxTriggersAddPost
   */
  static readonly ApiSecHomeBoxTriggersAddPostPath = '/api/sec/home-box/triggers/add';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxTriggersAddPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecHomeBoxTriggersAddPost$Response(params?: {
    body?: AddHomeBoxTriggerDto
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxTriggersAddPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSecHomeBoxTriggersAddPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecHomeBoxTriggersAddPost(params?: {
    body?: AddHomeBoxTriggerDto
  }): Observable<void> {

    return this.apiSecHomeBoxTriggersAddPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecHomeBoxTriggersIdDeleteDelete
   */
  static readonly ApiSecHomeBoxTriggersIdDeleteDeletePath = '/api/sec/home-box/triggers/{id}/delete';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxTriggersIdDeleteDelete()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersIdDeleteDelete$Response(params: {
    id: string;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxTriggersIdDeleteDeletePath, 'delete');
    if (params) {
      rb.path('id', params.id, {});
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
   * To access the full response (for headers, for example), `apiSecHomeBoxTriggersIdDeleteDelete$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersIdDeleteDelete(params: {
    id: string;
  }): Observable<void> {

    return this.apiSecHomeBoxTriggersIdDeleteDelete$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
