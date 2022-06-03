import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { GatewaySessionDto } from 'src/api/models';
import { HomeBoxService } from '../../api/services';

@Component({
  selector: 'app-home-box-settings',
  templateUrl: './home-box-settings.component.html'
})
export class HomeBoxSettingsComponent implements OnInit, OnDestroy {

  private subscriptions: Subscription[] = [];
  inputHomeBoxUser: string = '';
  inputHomeBoxPassword: string = '';
  gatewaySessions: GatewaySessionDto[] = [];

  constructor(private homeBoxService: HomeBoxService) {
  }

  ngOnInit() {
    this.reloadList();
  }

  reloadList() {
    this.subscriptions.push(this.homeBoxService.apiSHomeBoxGatewaySessionsGet$Json()
      .subscribe(keys => {
        this.gatewaySessions = keys;
      }));
  }

  onRefreshSession(session: GatewaySessionDto) {
    this.subscriptions.push(this.homeBoxService.apiSHomeBoxRefreshGatewaySessionPost({
      body: {
        id: session.id,
      }
    }).subscribe(() => this.reloadList()));
  }

  onLogin() {
    this.subscriptions.push(this.homeBoxService.apiSHomeBoxLoginToGatewayPost({
      body: {
        user: this.inputHomeBoxUser,
        password: this.inputHomeBoxPassword
      }
    }).subscribe(() => this.reloadList()));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}
