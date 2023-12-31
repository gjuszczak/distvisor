import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { UserSignedIn, UserSignedOut } from '../state/core.actions';
import { EMPTY } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private readonly store: Store) { }

  public init() {
    //return this.msalBroadcastService.inProgress$
    //  .pipe(
    //    filter((status: InteractionStatus) => status === InteractionStatus.None),
    //  )
    //  .subscribe(() => {
    //    let activeAccount = this.msalService.instance.getActiveAccount();

    //    if (!activeAccount) {
    //      const allAccounts = this.msalService.instance.getAllAccounts();
    //      if (allAccounts.length > 0) {
    //        this.msalService.instance.setActiveAccount(allAccounts[0]);
    //        activeAccount = this.msalService.instance.getActiveAccount();
    //      }
    //    }

    //    if (activeAccount) {
    //      this.store.dispatch(new UserSignedIn(activeAccount?.name ?? '', activeAccount?.username ?? ''))
    //    }
    //  });
    return EMPTY;
  }

  public login() {
    this.store.dispatch(new UserSignedIn('todo', 'todo'));
  }

  public logout() {
    this.store.dispatch(new UserSignedOut());
  }
}
