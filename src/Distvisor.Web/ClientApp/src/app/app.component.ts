import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { map, skipWhile } from 'rxjs/operators';
import { NavigationService } from './navigation.service';
import { AuthService } from '../auth/auth.service';
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
    this.navigationService.registerApp({
      name: 'Distvisor',
      icon: 'pi pi-home',
      routerLink: '/'
    });

    this.navigationService.registerApp({
      name: 'Login',
      icon: 'pi pi-sign-in',
      routerLink: '/auth/login',
    });

    this.navigationService.registerApp({
      name: 'Notifications',
      icon: 'pi pi-bell',
      routerLink: '/notifications',
      menuVisibile: this.authService.isAuthenticated()
    });

    this.navigationService.registerApp({
      name: 'Invoices',
      icon: 'pi pi-dollar',
      routerLink: '/invoices',
      menuVisibile: this.authService.isAuthenticated()
    });

    this.navigationService.registerApp({
      name: 'Settings',
      icon: 'pi pi-cog',
      routerLink: '/settings',
      menuVisibile: this.authService.isAuthenticated()
    });

    this.navigationService.registerApp({
      name: 'Logout',
      icon: 'pi pi-sign-out',
      routerLink: '/auth/logout',
      menuVisibile: this.authService.isAuthenticated()
    });
  }

  configureNotifications() {
    this.authSubscription = this.authService.isAuthenticated()
      .pipe(skipWhile(auth => auth === false))
      .subscribe(auth => {
        if (auth) {
          this.signalrService.connect(this.baseUrl, this.authService.getUser().accessToken);
        }
        else {
          this.signalrService.disconnect();
        }
      });
  }
}
