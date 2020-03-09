import { Component } from '@angular/core';
import { NotificationsService, SuccessNotification } from '../notifications.service';

@Component({
  selector: 'app-notifications-page',
  templateUrl: './notifications-page.component.html'
})
export class NotificationsPageComponent {

  constructor(private notificationsService: NotificationsService) {
  }

  onSingle() {
    this.notificationsService.show(<SuccessNotification>{ message: 'success' });
  }

  onMultiple() {
    this.notificationsService.show(<SuccessNotification>{ message: 'one' });
    this.notificationsService.show(<SuccessNotification>{ message: 'two' });
  }
}