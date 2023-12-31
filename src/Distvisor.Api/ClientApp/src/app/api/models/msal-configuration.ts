/* tslint:disable */
/* eslint-disable */
import { AuthMsalConfiguration } from './auth-msal-configuration';
import { CacheMsalConfiguration } from './cache-msal-configuration';
export interface MsalConfiguration {
  auth?: AuthMsalConfiguration;
  cache?: CacheMsalConfiguration;
}
