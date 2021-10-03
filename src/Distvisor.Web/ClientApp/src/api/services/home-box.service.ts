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

import { HomeBoxApiLoginDto } from '../models/home-box-api-login-dto';
import { HomeBoxDeviceDto } from '../models/home-box-device-dto';
import { HomeBoxTriggerDto } from '../models/home-box-trigger-dto';
import { UpdateHomeBoxDeviceDto } from '../models/update-home-box-device-dto';

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
   * Path part for operation apiSecHomeBoxApiLoginPost
   */
  static readonly ApiSecHomeBoxApiLoginPostPath = '/api/sec/home-box/api-login';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxApiLoginPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecHomeBoxApiLoginPost$Response(params?: {
    body?: HomeBoxApiLoginDto
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxApiLoginPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSecHomeBoxApiLoginPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecHomeBoxApiLoginPost(params?: {
    body?: HomeBoxApiLoginDto
  }): Observable<void> {

    return this.apiSecHomeBoxApiLoginPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
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
  }): Observable<StrictHttpResponse<Array<HomeBoxDeviceDto>>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxDevicesGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<HomeBoxDeviceDto>>;
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
  }): Observable<Array<HomeBoxDeviceDto>> {

    return this.apiSecHomeBoxDevicesGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<Array<HomeBoxDeviceDto>>) => r.body as Array<HomeBoxDeviceDto>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxDevicesGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxDevicesGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<Array<HomeBoxDeviceDto>>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxDevicesGetPath, 'get');
    if (params) {
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<Array<HomeBoxDeviceDto>>;
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
  }): Observable<Array<HomeBoxDeviceDto>> {

    return this.apiSecHomeBoxDevicesGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<Array<HomeBoxDeviceDto>>) => r.body as Array<HomeBoxDeviceDto>)
    );
  }

  /**
   * Path part for operation apiSecHomeBoxDevicesIdentifierUpdateDetailsPost
   */
  static readonly ApiSecHomeBoxDevicesIdentifierUpdateDetailsPostPath = '/api/sec/home-box/devices/{identifier}/updateDetails';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxDevicesIdentifierUpdateDetailsPost$Plain()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecHomeBoxDevicesIdentifierUpdateDetailsPost$Plain$Response(params: {
    identifier: string;
    body?: UpdateHomeBoxDeviceDto
  }): Observable<StrictHttpResponse<HomeBoxDeviceDto>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxDevicesIdentifierUpdateDetailsPostPath, 'post');
    if (params) {
      rb.path('identifier', params.identifier, {});
      rb.body(params.body, 'application/*+json');
    }

    return this.http.request(rb.build({
      responseType: 'text',
      accept: 'text/plain'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<HomeBoxDeviceDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSecHomeBoxDevicesIdentifierUpdateDetailsPost$Plain$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecHomeBoxDevicesIdentifierUpdateDetailsPost$Plain(params: {
    identifier: string;
    body?: UpdateHomeBoxDeviceDto
  }): Observable<HomeBoxDeviceDto> {

    return this.apiSecHomeBoxDevicesIdentifierUpdateDetailsPost$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<HomeBoxDeviceDto>) => r.body as HomeBoxDeviceDto)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxDevicesIdentifierUpdateDetailsPost$Json()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecHomeBoxDevicesIdentifierUpdateDetailsPost$Json$Response(params: {
    identifier: string;
    body?: UpdateHomeBoxDeviceDto
  }): Observable<StrictHttpResponse<HomeBoxDeviceDto>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxDevicesIdentifierUpdateDetailsPostPath, 'post');
    if (params) {
      rb.path('identifier', params.identifier, {});
      rb.body(params.body, 'application/*+json');
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json'
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<HomeBoxDeviceDto>;
      })
    );
  }

  /**
   * This method provides access to only to the response body.
   * To access the full response (for headers, for example), `apiSecHomeBoxDevicesIdentifierUpdateDetailsPost$Json$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecHomeBoxDevicesIdentifierUpdateDetailsPost$Json(params: {
    identifier: string;
    body?: UpdateHomeBoxDeviceDto
  }): Observable<HomeBoxDeviceDto> {

    return this.apiSecHomeBoxDevicesIdentifierUpdateDetailsPost$Json$Response(params).pipe(
      map((r: StrictHttpResponse<HomeBoxDeviceDto>) => r.body as HomeBoxDeviceDto)
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
   * Path part for operation apiSecHomeBoxTriggersGet
   */
  static readonly ApiSecHomeBoxTriggersGetPath = '/api/sec/home-box/triggers';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxTriggersGet$Plain()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersGet$Plain$Response(params?: {
  }): Observable<StrictHttpResponse<Array<HomeBoxTriggerDto>>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxTriggersGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiSecHomeBoxTriggersGet$Plain$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersGet$Plain(params?: {
  }): Observable<Array<HomeBoxTriggerDto>> {

    return this.apiSecHomeBoxTriggersGet$Plain$Response(params).pipe(
      map((r: StrictHttpResponse<Array<HomeBoxTriggerDto>>) => r.body as Array<HomeBoxTriggerDto>)
    );
  }

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxTriggersGet$Json()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersGet$Json$Response(params?: {
  }): Observable<StrictHttpResponse<Array<HomeBoxTriggerDto>>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxTriggersGetPath, 'get');
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
   * To access the full response (for headers, for example), `apiSecHomeBoxTriggersGet$Json$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersGet$Json(params?: {
  }): Observable<Array<HomeBoxTriggerDto>> {

    return this.apiSecHomeBoxTriggersGet$Json$Response(params).pipe(
      map((r: StrictHttpResponse<Array<HomeBoxTriggerDto>>) => r.body as Array<HomeBoxTriggerDto>)
    );
  }

  /**
   * Path part for operation apiSecHomeBoxTriggersPost
   */
  static readonly ApiSecHomeBoxTriggersPostPath = '/api/sec/home-box/triggers';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxTriggersPost()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecHomeBoxTriggersPost$Response(params?: {
    body?: HomeBoxTriggerDto
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxTriggersPostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSecHomeBoxTriggersPost$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecHomeBoxTriggersPost(params?: {
    body?: HomeBoxTriggerDto
  }): Observable<void> {

    return this.apiSecHomeBoxTriggersPost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecHomeBoxTriggersIdPut
   */
  static readonly ApiSecHomeBoxTriggersIdPutPath = '/api/sec/home-box/triggers/{id}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxTriggersIdPut()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecHomeBoxTriggersIdPut$Response(params: {
    id: string;
    body?: HomeBoxTriggerDto
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxTriggersIdPutPath, 'put');
    if (params) {
      rb.path('id', params.id, {});
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
   * To access the full response (for headers, for example), `apiSecHomeBoxTriggersIdPut$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  apiSecHomeBoxTriggersIdPut(params: {
    id: string;
    body?: HomeBoxTriggerDto
  }): Observable<void> {

    return this.apiSecHomeBoxTriggersIdPut$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecHomeBoxTriggersIdDelete
   */
  static readonly ApiSecHomeBoxTriggersIdDeletePath = '/api/sec/home-box/triggers/{id}';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxTriggersIdDelete()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersIdDelete$Response(params: {
    id: string;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxTriggersIdDeletePath, 'delete');
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
   * To access the full response (for headers, for example), `apiSecHomeBoxTriggersIdDelete$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersIdDelete(params: {
    id: string;
  }): Observable<void> {

    return this.apiSecHomeBoxTriggersIdDelete$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecHomeBoxTriggersIdExecutePost
   */
  static readonly ApiSecHomeBoxTriggersIdExecutePostPath = '/api/sec/home-box/triggers/{id}/execute';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxTriggersIdExecutePost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersIdExecutePost$Response(params: {
    id: string;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxTriggersIdExecutePostPath, 'post');
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
   * To access the full response (for headers, for example), `apiSecHomeBoxTriggersIdExecutePost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersIdExecutePost(params: {
    id: string;
  }): Observable<void> {

    return this.apiSecHomeBoxTriggersIdExecutePost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

  /**
   * Path part for operation apiSecHomeBoxTriggersIdTogglePost
   */
  static readonly ApiSecHomeBoxTriggersIdTogglePostPath = '/api/sec/home-box/triggers/{id}/toggle';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `apiSecHomeBoxTriggersIdTogglePost()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersIdTogglePost$Response(params: {
    id: string;
    enable?: boolean;
  }): Observable<StrictHttpResponse<void>> {

    const rb = new RequestBuilder(this.rootUrl, HomeBoxService.ApiSecHomeBoxTriggersIdTogglePostPath, 'post');
    if (params) {
      rb.path('id', params.id, {});
      rb.query('enable', params.enable, {});
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
   * To access the full response (for headers, for example), `apiSecHomeBoxTriggersIdTogglePost$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  apiSecHomeBoxTriggersIdTogglePost(params: {
    id: string;
    enable?: boolean;
  }): Observable<void> {

    return this.apiSecHomeBoxTriggersIdTogglePost$Response(params).pipe(
      map((r: StrictHttpResponse<void>) => r.body as void)
    );
  }

}
