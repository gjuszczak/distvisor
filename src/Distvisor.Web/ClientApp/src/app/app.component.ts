import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/auth/user.service';
import { NavigationService } from 'src/navigation/navigation.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {

  constructor(
    private userService: UserService,
    private navigationService: NavigationService) {
    this.configureNavigation();
  }

  configureNavigation() {
    this.navigationService.registerApp({
      name: 'Settings',
      icon: 'pi pi-cog',
      routerLink: '/settings',
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
}
