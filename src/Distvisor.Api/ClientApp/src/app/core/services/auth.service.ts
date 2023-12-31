import { Inject, Injectable } from '@angular/core';
import { MsalService, MsalBroadcastService, MSAL_GUARD_CONFIG, MsalGuardConfiguration } from '@azure/msal-angular';
import { filter } from 'rxjs/operators';
import { InteractionStatus, RedirectRequest } from '@azure/msal-browser';
import { Store } from '@ngxs/store';
import { UserSignedIn, UserSignedOut } from '../state/core.actions';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
    private msalService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
    private readonly store: Store) { }

  public init() {
    return this.msalBroadcastService.inProgress$
      .pipe(
        filter((status: InteractionStatus) => status === InteractionStatus.None),
      )
      .subscribe(() => {
        let activeAccount = this.msalService.instance.getActiveAccount();

        if (!activeAccount) {
          const allAccounts = this.msalService.instance.getAllAccounts();
          if (allAccounts.length > 0) {
            this.msalService.instance.setActiveAccount(allAccounts[0]);
            activeAccount = this.msalService.instance.getActiveAccount();
          }
        }

        if (activeAccount) {
          this.store.dispatch(new UserSignedIn(activeAccount?.name ?? '', activeAccount?.username ?? ''))
        }
      });
  }

  public login() {
    this.msalService.loginRedirect({ ...this.msalGuardConfig.authRequest } as RedirectRequest);
  }

  public logout() {
    this.store.dispatch(new UserSignedOut());
    this.msalService.logoutRedirect();
  }
}