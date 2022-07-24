/* tslint:disable */
/* eslint-disable */
import { NgModule, ModuleWithProviders, SkipSelf, Optional } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiConfiguration, ApiConfigurationParams } from './api-configuration';

import { AccountService } from './services/account.service';
import { AdminService } from './services/admin.service';
import { ClientConfigService } from './services/client-config.service';
import { CoreService } from './services/core.service';
import { EventLogService } from './services/event-log.service';
import { EventLogDeprService } from './services/event-log-depr.service';
import { FinancesService } from './services/finances.service';
import { HomeBoxService } from './services/home-box.service';
import { RedirectionsService } from './services/redirections.service';
import { RedirectToService } from './services/redirect-to.service';
import { RfLinkService } from './services/rf-link.service';
import { SecretsVaultService } from './services/secrets-vault.service';

/**
 * Module that provides all services and configuration.
 */
@NgModule({
  imports: [],
  exports: [],
  declarations: [],
  providers: [
    AccountService,
    AdminService,
    ClientConfigService,
    CoreService,
    EventLogService,
    EventLogDeprService,
    FinancesService,
    HomeBoxService,
    RedirectionsService,
    RedirectToService,
    RfLinkService,
    SecretsVaultService,
    ApiConfiguration
  ],
})
export class ApiModule {
  static forRoot(params: ApiConfigurationParams): ModuleWithProviders<ApiModule> {
    return {
      ngModule: ApiModule,
      providers: [
        {
          provide: ApiConfiguration,
          useValue: params
        }
      ]
    }
  }

  constructor( 
    @Optional() @SkipSelf() parentModule: ApiModule,
    @Optional() http: HttpClient
  ) {
    if (parentModule) {
      throw new Error('ApiModule is already loaded. Import in your base AppModule only.');
    }
    if (!http) {
      throw new Error('You need to import the HttpClientModule in your AppModule! \n' +
      'See also https://github.com/angular/angular/issues/20575');
    }
  }
}
