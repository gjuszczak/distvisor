import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/auth/user.service';
import { NavigationService } from 'src/navigation/navigation.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  constructor(
    private userService: UserService,
    private navigationService: NavigationService) { }

  ngOnInit() {
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
    });
  }
}
