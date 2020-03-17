import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ApplicationPaths, ReturnUrlType } from '../auth.constants';
import { AuthService } from '../auth.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
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
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.returnUrl = this.getReturnUrl();
  }

  onLogin() {
    this.isBusy = true;
    this.subscription = this.authService.login(this.username, this.password)
      .subscribe(
        _ => this.onAuthSuccess(),
        err => this.onAuthFail(err)
      );
  }

  private onAuthSuccess() {
    this.router.navigateByUrl(this.returnUrl);
  }

  private onAuthFail(response: HttpErrorResponse) {
    this.errorMessage = response.error;
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
