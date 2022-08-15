import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';
import { GatewaySessionDto } from 'src/app/api/models';
import { deleteGatewaySession, loadGatewaySessions, loginToGateway, refreshGatewaySession } from '../../../root-store/settings-store/actions';
import { selectGatewaySessions } from '../../../root-store/settings-store/home-box.selectors';
import { SettingsState } from '../../../root-store/settings-store/state';

@Component({
  selector: 'app-home-box-settings',
  templateUrl: './home-box-settings.component.html'
})
export class HomeBoxSettingsComponent {

  readonly gatewaySessions$ = this.store.select(selectGatewaySessions);

  inputHomeBoxUser: string = '';
  inputHomeBoxPassword: string = '';

  constructor(private readonly store: Store<SettingsState>) { }

  lazyLoadGatewaySessions(event: LazyLoadEvent) {
    this.store.dispatch(loadGatewaySessions({ firstOffset: event.first, pageSize: event.rows }));
  }

  onLogin() {
    this.store.dispatch(loginToGateway({
      command: {
        user: this.inputHomeBoxUser,
        password: this.inputHomeBoxPassword
      }
    }));
  }

  onRefreshSession(session: GatewaySessionDto) {
    this.store.dispatch(refreshGatewaySession({
      command: { sessionId: session.id }
    }));
  }

  onDeleteSession(session: GatewaySessionDto) {
    this.store.dispatch(deleteGatewaySession({
      command: { sessionId: session.id }
    }));
  }
}
