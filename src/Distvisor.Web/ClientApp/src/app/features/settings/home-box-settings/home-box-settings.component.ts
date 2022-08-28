import { Component } from '@angular/core';
import { Store, Select } from '@ngxs/store';
import { Observable } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';

import { GatewaySessionDto } from 'src/app/api/models';

import { GatewaySessionsState, GatewaySessionsStateModel } from '../store/gateway-sessions.state';
import {
  DeleteGatewaySession,
  LoadGatewaySessions,
  LoginToGateway,
  RefreshGatewaySession
} from '../store/gateway-sessions.actions';

@Component({
  selector: 'app-home-box-settings',
  templateUrl: './home-box-settings.component.html'
})
export class HomeBoxSettingsComponent {

  @Select(GatewaySessionsState.getGatewaySessions) readonly gatewaySessions$!: Observable<GatewaySessionsStateModel>;

  inputHomeBoxUser: string = '';
  inputHomeBoxPassword: string = '';

  constructor(private readonly store: Store) { }

  lazyLoadGatewaySessions({ first, rows }: LazyLoadEvent) {
    this.store.dispatch(new LoadGatewaySessions(first, rows));
  }

  onLogin() {
    this.store.dispatch(new LoginToGateway({
      user: this.inputHomeBoxUser,
      password: this.inputHomeBoxPassword
    }));
  }

  onRefreshSession(session: GatewaySessionDto) {
    this.store.dispatch(new RefreshGatewaySession({ sessionId: session.id }));
  }

  onDeleteSession(session: GatewaySessionDto) {
    this.store.dispatch(new DeleteGatewaySession({ sessionId: session.id }));
  }
}
