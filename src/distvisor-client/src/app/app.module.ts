import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LOCALE_ID, NgModule } from '@angular/core';
import { registerLocaleData } from '@angular/common';

import localePl from '@angular/common/locales/pl';
registerLocaleData(localePl);

import { NgxsModule } from '@ngxs/store';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';

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

import { BackupsModule } from './features/backups/backups.module';
import { EventLogModule } from './features/event-log/event-log.module';
import { FinancesModule } from './features/finances/finances.module';
import { HomeBoxModule } from './features/home-box/home-box.module';
import { RedirectionsModule } from './features/redirections/redirections.module';

import { CoreState } from './core/state/core.state';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,

    // Cookie consent
    NgcCookieConsentModule.forRoot({ cookie: { domain: '' }, enabled: false }),

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
    BackupsModule,
    CoreModule,
    EventLogModule,
    FinancesModule,
    HomeBoxModule,
    RedirectionsModule,

    // top level routing
    AppRoutingModule,
  ],
  providers: [
    { provide: LOCALE_ID, useValue: navigator.language === 'pl' ? 'pl' : 'en-US' },
    MessageService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
