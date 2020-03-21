import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { ApplicationPaths } from '../auth.constants';
import { AuthService } from '../auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html'
})
export class LogoutComponent implements OnInit, OnDestroy {

  constructor(
    private authService: AuthService,
    private router: Router) { }

  private subscription : Subscription;

  ngOnInit() {
    this.subscription = this.authService.logout()
      .subscribe(
        _=> this.finalizeLogout(),
        _=> this.finalizeLogout()
      );
  }

  finalizeLogout() {
    this.router.navigateByUrl(ApplicationPaths.DefaultLoginRedirectPath);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
