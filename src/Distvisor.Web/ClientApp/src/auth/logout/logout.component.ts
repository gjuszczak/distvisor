import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { ApplicationPaths } from '../auth.constants';
import { UserService } from '../user.service';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/api/services';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html'
})
export class LogoutComponent implements OnInit, OnDestroy {

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router) { }

  private subscription : Subscription;

  ngOnInit() {
    this.subscription = this.authService.apiAuthLogoutPost()
      .subscribe(
        _=> this.finalizeLogout(),
        _=> this.finalizeLogout()
      );
  }

  finalizeLogout() {
    this.userService.clearUser();
    this.router.navigate(ApplicationPaths.LoginPathComponents);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
