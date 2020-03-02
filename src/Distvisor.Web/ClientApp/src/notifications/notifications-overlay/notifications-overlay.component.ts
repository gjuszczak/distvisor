import { Component, OnInit, Inject } from '@angular/core';
import { NotificationsService } from '../notifications.service';

import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-notifications-overlay',
  templateUrl: './notifications-overlay.component.html'
})
export class NotificationsOverlayComponent implements OnInit {

  constructor(
    private notificationsService: NotificationsService,
    @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
    const baseUrlClean = this.baseUrl.replace(/\/$/, "");

    const connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(`${baseUrlClean}/notificationshub`)
      .build();

    connection.start().then(function () {
      console.log('Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("ReceiveMessage", (type: string, payload: string) => {
      this.notificationsService.addSingle();
    });
  }
}