import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ServiceWorkerModule } from '@angular/service-worker';

import { MsalModule, MsalInterceptor, MsalService, MsalGuard, MsalBroadcastService, MsalRedirectComponent, MSAL_INSTANCE, MSAL_GUARD_CONFIG, MSAL_INTERCEPTOR_CONFIG } from '@azure/msal-angular';
import { MsalGuardConfigFactory, MsalInstanceFactory, MsalInterceptorConfigFactory } from './msal-integration';

import { NgcCookieConsentModule } from 'ngx-cookieconsent';

import { MessageService } from 'primeng/api';
import { MenuModule } from 'primeng/menu';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';

import { environment } from '../environments/environment';

import { ApiModule } from './api/api.module';
import { RootStoreModule } from './root-store';
import { AppRoutingModule } from './app-routing.module';

import { SettingsModule } from './modules/settings/settings.module';
import { FinancesModule } from './modules/finances/finances.module';
import { HomeBoxModule } from './modules/home-box/home-box.module';
import { EventLogModule } from './modules/event-log/event-log.module';
import { SignalrModule } from './modules/signalr/signalr.module';

import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { FooterComponent } from './components/footer/footer.component';
import { PrivacyPolicyComponent } from './components/privacy-policy/privacy-policy.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { LogoutComponent } from './components/logout/logout.component';
import { NavigationService } from './services/navigation.service';
import { AuthService } from './services/auth.service';

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

    // Cookie consent
    NgcCookieConsentModule.forRoot({ cookie: { domain: '' }, enabled: false }),

    // Msal
    MsalModule,

    // PrimeNg
    MenuModule,
    ButtonModule,
    RippleModule,
    ToastModule,

    // internal
    ApiModule,
    RootStoreModule,
    AppRoutingModule,
    SettingsModule,
    FinancesModule,
    HomeBoxModule,
    SignalrModule,
    EventLogModule
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
