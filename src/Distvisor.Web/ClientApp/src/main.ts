import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { MSAL_CONFIG, MSAL_CONFIG_ANGULAR, MsalAngularConfiguration } from '@azure/msal-angular';
import { Configuration as MsalConfiguration } from 'msal';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

export function getMsalConfig(clientConfig: any): MsalConfiguration {
  const config = clientConfig['msal'] || {};
  return (config as MsalConfiguration);
}

export function getMsalConfigAngular(clientConfig: any): MsalAngularConfiguration {
  const config = clientConfig['msalAngular'] || {};
  return (config as MsalAngularConfiguration);
}

if (environment.production) {
  enableProdMode();
}

window.fetch('api/clientConfig')
  .then(res => res.json())
  .then((resp) => {
    const providers = [
      { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] },
      { provide: MSAL_CONFIG, useValue: getMsalConfig(resp) },
      { provide: MSAL_CONFIG_ANGULAR, useValue: getMsalConfigAngular(resp) }
    ];

    platformBrowserDynamic(providers).bootstrapModule(AppModule)
      .catch(err => console.log(err));
  })