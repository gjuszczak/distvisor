import { Injectable } from '@angular/core';
import { MsalService, BroadcastService } from '@azure/msal-angular';
import { Account } from 'msal';
import { Observable, BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private isAuthenticatedSubject: DistinctBehaviorSubject<boolean>;
  private accessTokenSubject: DistinctBehaviorSubject<string | null>;

  constructor(
    private msalService: MsalService,
    private broadcastService: BroadcastService) {

    this.isAuthenticatedSubject = new DistinctBehaviorSubject(!!this.getUser());
    this.accessTokenSubject = new DistinctBehaviorSubject(null);

    this.broadcastService.subscribe('msal:loginSuccess', () => {
      this.isAuthenticatedSubject.next(true);
    });

    this.broadcastService.subscribe('msal:loginFailure', () => {
      this.isAuthenticatedSubject.next(true);
    });

    this.broadcastService.subscribe("msal:acquireTokenSuccess", payload => {
      this.accessTokenSubject.next(payload['accessToken']);
    });
    
    this.broadcastService.subscribe("msal:acquireTokenFailure", payload => {
      this.accessTokenSubject.next(null);
    });

    this.msalService.handleRedirectCallback((authError, response) => {
      if (authError) {
        console.error('Redirect Error: ', authError.errorMessage);
        return;
      }

      console.log('Redirect Success: ', response);
    });
  }

  public isAuthenticated(): Observable<boolean> {
    return this.isAuthenticatedSubject;
  }

  public accessToken(): Observable<string | null> {
    return this.accessTokenSubject;
  }

  public getUser(): Account | null {
    return this.msalService.getAccount();
  }

  public login() {
    this.msalService.loginRedirect();
  }

  public logout() {
    this.msalService.logout();
    this.isAuthenticatedSubject.next(false);
  }
}

class DistinctBehaviorSubject<T> extends BehaviorSubject<T> {
  public next(value: T): void {
      if (value !== this.value)
          super.next(value);
  }
}
