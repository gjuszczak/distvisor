import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { skipWhile } from 'rxjs/operators';
import { NavigationService } from './navigation.service';
import { AuthService } from './auth.service';
import { ApiConfiguration } from '../api/api-configuration';
import { SignalrService } from '../notifications/signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {

  private authSubscription: Subscription;

  constructor(
    private authService: AuthService,
    private navigationService: NavigationService,
    private apiConfiguration: ApiConfiguration,
    private signalrService: SignalrService,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.configureApi();
    this.configureNavigation();
    this.configureNotifications();
  }

  ngOnDestroy() {
    this.authSubscription.unsubscribe();
  }

  configureApi() {
    this.apiConfiguration.rootUrl = this.baseUrl.replace(/\/$/, "");
  }

  configureNavigation() {
    this.navigationService.registerNavMenuItem({
      name: 'Distvisor',
      icon: 'pi pi-home',
      routerLink: '/'
    });

    this.navigationService.registerNavMenuItem({
      name: 'Invoices',
      icon: 'pi pi-dollar',
      routerLink: '/invoices',
      menuVisibile: this.authService.isInUserRole()
    });

    this.navigationService.registerNavMenuItem({
      name: 'Event Log',
      icon: 'pi pi-directions',
      routerLink: '/event-log',
      menuVisibile: this.authService.isInUserRole()
    });

    this.navigationService.registerNavMenuItem({
      name: 'Settings',
      icon: 'pi pi-sliders-h',
      routerLink: '/settings',
      menuVisibile: this.authService.isInUserRole()
    });

    this.navigationService.registerNavMenuItem({
      name: 'Logout',
      icon: 'pi pi-sign-out',
      routerLink: '/logout',
      menuVisibile: this.authService.isAuthenticated()
    });
  }

  configureNotifications() {
    this.authSubscription = this.authService.accessToken()
      .subscribe(token => {
        if (token) {
          this.signalrService.connect(this.baseUrl, token);
        }
        else {
          this.signalrService.disconnect();
        }
      });
  }
}
