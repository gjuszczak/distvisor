import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ServiceWorkerModule } from '@angular/service-worker';

import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

import { MsalModule, MsalInterceptor, MsalService, MsalGuard, MsalBroadcastService, MsalRedirectComponent, MSAL_INSTANCE, MSAL_GUARD_CONFIG, MSAL_INTERCEPTOR_CONFIG } from '@azure/msal-angular';
import { MsalGuardConfigFactory, MsalInstanceFactory, MsalInterceptorConfigFactory } from './msal-integration';

import { NgcCookieConsentModule } from 'ngx-cookieconsent';

import { MessageService } from 'primeng/api';
import { MenuModule } from 'primeng/menu';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';

import { environment } from '../environments/environment';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { SettingsModule } from '../settings/settings.module';
import { FinancesModule } from '../finances/finances.module';
import { HomeBoxModule } from '../home-box/home-box.module';
import { EventLogModule } from '../event-log/event-log.module';
import { ApiModule } from '../api/api.module';
import { SignalrModule } from '../signalr/signalr.module';
import { FooterComponent } from './footer/footer.component';
import { PrivacyPolicyComponent } from './privacy-policy/privacy-policy.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { NavigationService } from './navigation.service';
import { AuthService } from './auth.service';
import { LogoutComponent } from './logout/logout.component';
import { metaReducers, reducers } from './state';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LogoutComponent,
    NavMenuComponent,
    FooterComponent,
    PrivacyPolicyComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: 'logout', component: LogoutComponent, pathMatch: 'full' },
      { path: 'privacy-policy', component: PrivacyPolicyComponent, pathMatch: 'full' },
      { path: '**', component: HomeComponent, pathMatch: 'full' },
    ]),
    NgcCookieConsentModule.forRoot({ cookie: { domain: '' }, enabled: false }),

    //NgRx
    StoreModule.forRoot({ }),
    EffectsModule.forRoot(),

    //NgRx dev tools
    StoreModule.forRoot(reducers, { metaReducers }),
    !environment.production ? StoreDevtoolsModule.instrument() : [],

    // Msal
    MsalModule,

    // PrimeNg
    MenuModule,
    ButtonModule,
    RippleModule,
    ToastModule,

    // internal
    SettingsModule,
    FinancesModule,
    HomeBoxModule,
    SignalrModule,
    EventLogModule,
    ApiModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: MsalInterceptor, multi: true },
    { provide: MSAL_INSTANCE, useFactory: MsalInstanceFactory, deps: ['CLIENT_CONFIGURATION'] },
    { provide: MSAL_GUARD_CONFIG, useFactory: MsalGuardConfigFactory, deps: ['CLIENT_CONFIGURATION'] },
    { provide: MSAL_INTERCEPTOR_CONFIG, useFactory: MsalInterceptorConfigFactory, deps: ['CLIENT_CONFIGURATION'] },
    MsalService,
    MsalGuard,
    MsalBroadcastService,
    NavigationService,
    AuthService,
    MessageService,
  ],
  bootstrap: [AppComponent, MsalRedirectComponent]
})
export class AppModule { }
