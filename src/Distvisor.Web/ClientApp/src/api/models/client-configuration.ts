/* tslint:disable */
import { BackendDetails } from './backend-details';
import { MsalAngularConfiguration } from './msal-angular-configuration';
import { MsalConfiguration } from './msal-configuration';
export interface ClientConfiguration {
  backendDetails?: BackendDetails;
  msal?: MsalConfiguration;
  msalAngular?: MsalAngularConfiguration;
}
