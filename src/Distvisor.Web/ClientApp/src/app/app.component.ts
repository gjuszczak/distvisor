import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { UserService } from 'src/auth/user.service';
import { NavigationService } from 'src/navigation/navigation.service';
import { map } from 'rxjs/operators';
import { ApiConfiguration } from 'src/api/api-configuration';
import { SignalrService } from 'src/notifications/signalr.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit, OnDestroy {

  private authSubscription: Subscription;

  constructor(
    private userService: UserService,
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
      name: 'Home',
      icon: 'pi pi-home',
      routerLink: '/'
    });

    this.navigationService.registerApp({
      name: 'Notifications',
      icon: 'pi pi-bell',
      routerLink: '/notifications',
      visibile: this.userService.isAuthenticated()
    });

    this.navigationService.registerApp({
      name: 'Invoices',
      icon: 'pi pi-dollar',
      routerLink: '/invoices',
      visibile: this.userService.isAuthenticated()
    });

    this.navigationService.registerApp({
      name: 'Settings',
      icon: 'pi pi-cog',
      routerLink: '/settings',
      visibile: this.userService.isAuthenticated()
    });

    this.navigationService.registerApp({
      name: 'Logout',
      icon: 'pi pi-sign-out',
      routerLink: '/auth/logout',
      visibile: this.userService.isAuthenticated()
    });

    this.navigationService.registerApp({
      name: 'Login',
      icon: 'pi pi-sign-in',
      routerLink: '/auth/login',
      visibile: this.userService.isAuthenticated().pipe(
        map(auth => !auth)),
    });
  }

  configureNotifications() {
    this.authSubscription = this.userService.getUser()
      .subscribe(user => {
        if (user != null) {
          this.signalrService.connect(this.baseUrl, user.sessionId);
        }
        else {
          this.signalrService.disconnect();
        }
      });
  }
}
