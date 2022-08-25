import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { GatewaySessionDto } from 'src/app/api/models';
import { RootStoreState, SettingsStoreActions, SettingsStoreSelectors } from 'src/app/root-store';

@Component({
  selector: 'app-home-box-settings',
  templateUrl: './home-box-settings.component.html'
})
export class HomeBoxSettingsComponent {

  readonly gatewaySessions$ = this.store.select(
    SettingsStoreSelectors.selectGatewaySessions
  );

  inputHomeBoxUser: string = '';
  inputHomeBoxPassword: string = '';

  constructor(private readonly store: Store<RootStoreState.State>) { }

  lazyLoadGatewaySessions(event: LazyLoadEvent) {
    this.store.dispatch(SettingsStoreActions.loadGatewaySessions({
      firstOffset: event.first, 
      pageSize: event.rows
    }));
  }

  onLogin() {
    this.store.dispatch(SettingsStoreActions.loginToGateway({
      command: {
        user: this.inputHomeBoxUser,
        password: this.inputHomeBoxPassword
      }
    }));
  }

  onRefreshSession(session: GatewaySessionDto) {
    this.store.dispatch(SettingsStoreActions.refreshGatewaySession({
      command: { sessionId: session.id }
    }));
  }

  onDeleteSession(session: GatewaySessionDto) {
    this.store.dispatch(SettingsStoreActions.deleteGatewaySession({
      command: { sessionId: session.id }
    }));
  }
}
