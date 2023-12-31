import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { Store } from '@ngxs/store';
import { NgcCookieConsentService } from 'ngx-cookieconsent';
import { PrimeNGConfig } from 'primeng/api';
import { ApiConfiguration } from './api/api-configuration';
import { AuthService } from './core/services/auth.service';
import { ConfigureNavigation } from './core/state/core.actions';
import { UserRole } from './core/state/core.state';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {

  private subscriptions: Subscription[] = [];

  constructor(
    private readonly store: Store,
    private authService: AuthService,
    private apiConfiguration: ApiConfiguration,
    private ccService: NgcCookieConsentService,
    private primengConfig: PrimeNGConfig,
    @Inject('BASE_URL') private baseUrl: string,
    @Inject('HOSTNAME') private hostname: string) {
  }

  ngOnInit() {
    this.configureAuthentication();
    this.configurePrimeNg();
    this.configureApi();
    this.configureNavigation();
    this.configureCookieConsent();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(s => s.unsubscribe());
  }

  configureAuthentication() {
    //this.subscriptions.push(this.authService.init());
  }

  configurePrimeNg() {
    this.primengConfig.ripple = true;
  }

  configureApi() {
    this.apiConfiguration.rootUrl = this.baseUrl.replace(/\/$/, "");
  }

  configureNavigation() {
    this.store.dispatch(new ConfigureNavigation([
      {
        label: 'Start',
        items: [
          { label: 'Homepage', icon: 'pi-home', route: '/', routeExact: true },
          { label: 'Privacy Policy', icon: 'pi-info-circle', route: '/privacy-policy', routeExact: true },
        ]
      },
      {
        label: 'Finances',
        items: [
          { label: 'Accounts', icon: 'pi-chart-line', route: '/finances', routeExact: true, minimalRole: UserRole.User },
        ]
      },
      {
        label: 'Home Box',
        items: [
          { label: 'Devices', icon: 'pi-sun', route: '/home-box/devices', routeExact: true, minimalRole: UserRole.User },
          { label: 'Gateway Sessions', icon: 'pi-unlock', route: '/home-box/gateway-sessions', routeExact: true, minimalRole: UserRole.User },
        ]
      },
      {
        label: 'Admin',
        items: [
          { label: 'Event Log', icon: 'pi-directions', route: '/admin/event-log', routeExact: false, minimalRole: UserRole.Admin },
          { label: 'Backups', icon: 'pi-database', route: '/admin/backups', routeExact: true, minimalRole: UserRole.Admin },
          { label: 'Redirections', icon: 'pi-arrow-right-arrow-left', route: '/admin/redirections', routeExact: true, minimalRole: UserRole.Admin },
        ]
      },
      {
        label: 'User',
        items: [
          { label: 'Logout', icon: 'pi-sign-out', route: '/logout', routeExact: true, minimalRole: UserRole.None },
        ]
      },
    ]));
  }

  configureCookieConsent() {
    this.ccService.init({
      cookie: {
        domain: this.hostname
      },
      position: "bottom-right",
      theme: "classic",
      palette: {
        popup: {
          background: "#23272b",
          text: "#ffffff",
          link: "#ffffff"
        },
        button: {
          background: "#34A835",
          text: "#ffffff",
          border: "transparent"
        }
      },
      type: "info",
      content: {
        href: "/privacy-policy"
      }
    });
  }
}
