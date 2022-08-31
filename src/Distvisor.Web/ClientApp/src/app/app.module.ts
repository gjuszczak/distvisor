import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ServiceWorkerModule } from '@angular/service-worker';

import { NgxsModule } from '@ngxs/store';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';

import { MsalModule, MsalInterceptor, MsalService, MsalGuard, MsalBroadcastService, MsalRedirectComponent, MSAL_INSTANCE, MSAL_GUARD_CONFIG, MSAL_INTERCEPTOR_CONFIG } from '@azure/msal-angular';
import { MsalGuardConfigFactory, MsalInstanceFactory, MsalInterceptorConfigFactory } from './msal-integration';

import { NgcCookieConsentModule } from 'ngx-cookieconsent';

import { MessageService } from 'primeng/api';
import { MenuModule } from 'primeng/menu';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';

import { environment } from '../environments/environment';

import { AppRoutingModule } from './app-routing.module';
import { ApiModule } from './api/api.module';
import { CoreModule } from './core/core.module';

import { SettingsModule } from './features/settings/settings.module';
import { FinancesModule } from './features/finances/finances.module';
import { HomeBoxModule } from './features/home-box/home-box.module';
import { EventLogModule } from './features/event-log/event-log.module';

import { CoreState } from './core/state/core.state';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
    BrowserAnimationsModule,

    // Cookie consent
    NgcCookieConsentModule.forRoot({ cookie: { domain: '' }, enabled: false }),

    // Msal
    MsalModule,

    // Ngxs
    NgxsModule.forRoot([CoreState], {
      developmentMode: !environment.production,
      selectorOptions: {
        suppressErrors: false,
        injectContainerState: false
      }
    }),
    NgxsReduxDevtoolsPluginModule.forRoot(),

    // PrimeNg
    MenuModule,
    ButtonModule,
    RippleModule,
    ToastModule,

    // internal
    ApiModule,
    CoreModule,
    SettingsModule,
    FinancesModule,
    HomeBoxModule,
    EventLogModule,

    // top level routing
    AppRoutingModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: MsalInterceptor, multi: true },
    { provide: MSAL_INSTANCE, useFactory: MsalInstanceFactory, deps: ['CLIENT_CONFIGURATION'] },
    { provide: MSAL_GUARD_CONFIG, useFactory: MsalGuardConfigFactory, deps: ['CLIENT_CONFIGURATION'] },
    { provide: MSAL_INTERCEPTOR_CONFIG, useFactory: MsalInterceptorConfigFactory, deps: ['CLIENT_CONFIGURATION'] },
    MsalService,
    MsalGuard,
    MsalBroadcastService,
    MessageService,
  ],
  bootstrap: [AppComponent, MsalRedirectComponent]
})
export class AppModule { }
