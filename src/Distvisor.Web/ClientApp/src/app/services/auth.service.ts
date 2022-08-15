import { Inject, Injectable } from '@angular/core';
import { MsalService, MsalBroadcastService, MSAL_GUARD_CONFIG, MsalGuardConfiguration } from '@azure/msal-angular';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { AccountService } from 'src/app/api/services';
import { map, catchError, filter, mergeMap } from 'rxjs/operators';
import { AccountInfo, AuthenticationResult, EventMessage, EventType, InteractionStatus, RedirectRequest } from '@azure/msal-browser';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private isAuthenticatedSubject: DistinctBehaviorSubject<boolean>;
  private isInUserRoleSubject: DistinctBehaviorSubject<boolean>;
  private accessTokenSubject: DistinctBehaviorSubject<string | null>;

  constructor(
    @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
    private msalService: MsalService,
    private accountService: AccountService,
    private msalBroadcastService: MsalBroadcastService) {

    this.isAuthenticatedSubject = new DistinctBehaviorSubject<boolean>(false);
    this.isInUserRoleSubject = new DistinctBehaviorSubject<boolean>(false);
    this.accessTokenSubject = new DistinctBehaviorSubject<string | null>(null);

    this.msalBroadcastService.msalSubject$.pipe(
      filter((msg: EventMessage) => msg.eventType === EventType.LOGIN_SUCCESS),
    ).subscribe((result: EventMessage) => {
      console.log(result);
      const payload = result.payload as AuthenticationResult;
      this.msalService.instance.setActiveAccount(payload.account);
      this.accessTokenSubject.next(payload.accessToken);
    });

    this.msalBroadcastService.inProgress$.pipe(
      filter((status: InteractionStatus) => status === InteractionStatus.None),
      map(() => !!this.getUser())
    ).subscribe(isAuth => {
      this.isAuthenticatedSubject.next(isAuth);
    });

    this.msalBroadcastService.msalSubject$.pipe(
      filter(ev => ev.eventType === "msal:acquireTokenSuccess" || ev.eventType === "msal:acquireTokenFailure"),
      map(ev => (<AuthenticationResult>ev.payload)?.accessToken || null)
    ).subscribe(accessToken => {
      this.accessTokenSubject.next(accessToken);
    });

    this.isAuthenticatedSubject.pipe(
      mergeMap(authSuccess => {
        if (authSuccess) {
          return this.accountService.apiSecAccountGet$Json().pipe(
            map(acc => acc.role === "user"),
            catchError(_ => of(false))
          );
        }
        return of(false);
      })
    ).subscribe(isUser => {
      this.isInUserRoleSubject.next(isUser);
    });

    this.isAuthenticatedSubject.next(!!this.getUser());
  }

  public isAuthenticated(): Observable<boolean> {
    return this.isAuthenticatedSubject;
  }

  public isInUserRole(): Observable<boolean> {
    return this.isInUserRoleSubject;
  }

  public accessToken(): Observable<string | null> {
    return this.accessTokenSubject;
  }

  public getUser(): AccountInfo | null {
    let activeAccount = this.msalService.instance.getActiveAccount();

    if (!activeAccount && this.msalService.instance.getAllAccounts().length > 0) {
      let accounts = this.msalService.instance.getAllAccounts();
      this.msalService.instance.setActiveAccount(accounts[0]);
      activeAccount = accounts[0];
    }

    return activeAccount;
  }

  public login() {
    this.msalService.loginRedirect({ ...this.msalGuardConfig.authRequest } as RedirectRequest);
  }

  public logout() {
    this.msalService.logoutRedirect();
  }
}

class DistinctBehaviorSubject<T> extends BehaviorSubject<T> {
  public next(value: T): void {
    if (value !== this.value)
      super.next(value);
  }
}
