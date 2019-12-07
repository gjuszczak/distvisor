import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService, IAuthResult } from '../auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ApplicationPaths, ReturnUrlType } from '../auth.constants';
import { UserService } from '../user.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit, OnDestroy {
  private returnUrl: string;
  private subscription: Subscription;
  public username: string;
  public password: string;
  public errorMessage: string;
  public isBusy: boolean;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.returnUrl = this.getReturnUrl();
  }

  onLogin() {
    this.isBusy = true;
    this.subscription = this.authService.login(this.username, this.password)
      .subscribe(
        result => this.onAuthSuccess(result),
        err => this.onAuthFail(err));
  }

  private onAuthSuccess(result: IAuthResult) {
    this.userService.setUser(result.user)
    this.router.navigateByUrl(this.returnUrl);
  }

  private onAuthFail(result: IAuthResult) {
    this.errorMessage = result.message;
    this.isBusy = false;
  }

  private getReturnUrl(): string {
    const fromQuery = (this.activatedRoute.snapshot.queryParams as INavigationState).returnUrl;

    if (fromQuery && !(fromQuery.startsWith(`${window.location.origin}/`) || /\/[^\/].*/.test(fromQuery))) {
      throw new Error('Invalid return url. The return url needs to have the same origin as the current page.');
    }
    return fromQuery || ApplicationPaths.DefaultLoginRedirectPath;
  }

  ngOnDestroy(): void {
    if (this.subscription != null)
      this.subscription.unsubscribe();
  }
}

interface INavigationState {
  [ReturnUrlType]: string;
}
