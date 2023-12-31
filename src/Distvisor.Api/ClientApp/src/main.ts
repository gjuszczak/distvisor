import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import { BackendDetails, ClientConfiguration } from './app/api/models';

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

export function getHostname() {
  return location.hostname
}

function getBackendDetails(clientConfig: ClientConfiguration): BackendDetails {
  return clientConfig.backendDetails || {};
}

if (environment.production) {
  enableProdMode();
}

window.fetch('api/client-config')
  .then(res => res.json())
  .then((resp) => {
    const config = resp as ClientConfiguration;
    const providers = [
      { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] },
      { provide: 'HOSTNAME', useFactory: getHostname, deps: [] },
      { provide: 'CLIENT_CONFIGURATION', useValue: config },
      { provide: 'BACKEND_DETAILS', useValue: getBackendDetails(config) }
    ];

    platformBrowserDynamic(providers).bootstrapModule(AppModule)
      .catch(err => console.log(err));
  })