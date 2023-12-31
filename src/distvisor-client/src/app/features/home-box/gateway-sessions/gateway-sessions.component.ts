import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store, Select } from '@ngxs/store';
import { Observable } from 'rxjs';
import { MenuItem } from 'primeng/api';
import { TableLazyLoadEvent } from 'primeng/table';

import { GatewaySession, GatewaySessionsList, GatewaySessionsState } from '../store/gateway-sessions.state';
import { DeleteGatewaySession, LoadGatewaySessions, OpenGatewaySession, RefreshGatewaySession } from '../store/gateway-sessions.actions';

@Component({
  selector: 'app-gateway-sessions',
  templateUrl: './gateway-sessions.component.html'
})
export class GatewaySessionsComponent {

  @Select(GatewaySessionsState.getGatewaySessions)
  readonly gatewaySessions$!: Observable<GatewaySessionsList>;

  public inputHomeBoxUser: string = '';
  public inputHomeBoxPassword: string = '';

  public gatewaySessionMenuItems: MenuItem[] = [];
  public selectedGatewaySession?: GatewaySession;
  private firstLazyLoad: boolean = true;

  constructor(
    private readonly store: Store,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.gatewaySessionMenuItems = [
      {
        label: 'Refresh',
        icon: 'pi pi-refresh',
        command: () => {
          if (this.selectedGatewaySession) {
            this.refreshSession(this.selectedGatewaySession);
          }
        }
      },
      {
        label: 'Delete',
        icon: 'pi pi-trash',
        command: () => {
          if (this.selectedGatewaySession) {
            this.deleteSession(this.selectedGatewaySession);
          }
        }
      }
    ];

    this.route.queryParams.subscribe(({ first, rows }) => {
      this.reloadGatewaySessions(first, rows);
    });
  }

  toggleGatewaySessionMenu(menu: any, event: any, gatewaySession: GatewaySession) {
    this.selectedGatewaySession = gatewaySession;
    menu.toggle(event);
  }

  lazyLoadGatewaySessions({ first, rows }: TableLazyLoadEvent) {
    if (this.firstLazyLoad) {
      this.firstLazyLoad = false;
      return;
    }

    this.router.navigate([], { queryParams: { first, rows } });
  }

  reloadGatewaySessions(first?: number, rows?: number) {
    this.store.dispatch(new LoadGatewaySessions(first, rows));
  }

  onLogin() {
    this.store.dispatch(new OpenGatewaySession(this.inputHomeBoxUser, this.inputHomeBoxPassword));
  }

  refreshSession(session: GatewaySession) {
    this.store.dispatch(new RefreshGatewaySession(session.id));
  }

  deleteSession(session: GatewaySession) {
    this.store.dispatch(new DeleteGatewaySession(session.id));
  }
}
