import { Component } from '@angular/core';
import { NotificationsService } from '../notifications.service';

@Component({
  selector: 'app-notifications-page',
  templateUrl: './notifications-page.component.html'
})
export class NotificationsPageComponent {

  constructor(private notificationsService: NotificationsService) {
  }

  onSingle() {
    this.notificationsService.addSingle();
  }

  onMultiple() {
    this.notificationsService.addMultiple();
  }
}