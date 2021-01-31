import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ServiceWorkerModule } from '@angular/service-worker';

import { MsalModule, MsalInterceptor, MsalService } from '@azure/msal-angular';
import { NgcCookieConsentModule } from 'ngx-cookieconsent';

import { MenuModule } from 'primeng/menu';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';

import { environment } from '../environments/environment';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { SettingsModule } from '../settings/settings.module';
import { InvoicesModule } from '../invoices/invoices.module';
import { FinancesModule } from '../finances/finances.module';
import { EventLogModule } from '../event-log/event-log.module';
import { ApiModule } from '../api/api.module';
import { NotificationsModule } from '../notifications/notifications.module';
import { FooterComponent } from './footer/footer.component';
import { PrivacyPolicyComponent } from './privacy-policy/privacy-policy.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { NavigationService } from './navigation.service';
import { AuthService } from './auth.service';
import { LogoutComponent } from './logout/logout.component';

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

    // Msal
    MsalModule,

    // PrimeNg
    MenuModule,
    ButtonModule,
    RippleModule,

    // internal
    SettingsModule,
    InvoicesModule,
    FinancesModule,
    NotificationsModule,
    EventLogModule,
    ApiModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: MsalInterceptor, multi: true },
    MsalService,
    NavigationService,
    AuthService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
