/* tslint:disable */
/* eslint-disable */
import { BackendDetails } from './backend-details';
import { MsalConfiguration } from './msal-configuration';
import { MsalGuardConfiguration } from './msal-guard-configuration';
import { MsalInterceptorConfiguration } from './msal-interceptor-configuration';
import { MsalProtectedResource } from './msal-protected-resource';
export interface ClientConfiguration {
  backendDetails?: BackendDetails;
  msal?: MsalConfiguration;
  msalGuard?: MsalGuardConfiguration;
  msalInterceptor?: MsalInterceptorConfiguration;
  msalProtectedResources?: null | Array<MsalProtectedResource>;
}
