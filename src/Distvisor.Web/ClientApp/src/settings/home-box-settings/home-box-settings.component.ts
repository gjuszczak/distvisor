import { Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { HomeBoxService } from '../../api/services';

@Component({
  selector: 'app-home-box-settings',
  templateUrl: './home-box-settings.component.html'
})
export class HomeBoxSettingsComponent implements OnDestroy {

  private subscriptions: Subscription[] = [];
  inputHomeBoxUser: string = '';
  inputHomeBoxPassword: string = '';

  constructor(private homeBoxService: HomeBoxService) { }

  onSave() {
    this.subscriptions.push(this.homeBoxService.apiSecHomeBoxApiLoginPost({
      body:{
        user: this.inputHomeBoxUser,
        password: this.inputHomeBoxPassword
      }
    }).subscribe());
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}
