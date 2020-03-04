import { Injectable } from '@angular/core';
import { NotificationsService } from './notifications.service';

import * as signalR from '@aspnet/signalr';

@Injectable()
export class SignalrService {

  private connection: signalR.HubConnection;

  constructor(private notificationsService: NotificationsService) {
    this.connection = null;
  }

  connect(baseUrl: string, userSession: string) {
    const baseUrlClean = baseUrl.replace(/\/$/, "");
    this.connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(`${baseUrlClean}/notificationshub`, { accessTokenFactory: () => userSession })
      .build();

    this.connection.start().then(function () {
      console.log('Connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    this.connection.on("PushNotification", (payload: string) => {
      this.notificationsService.success(payload);
    });
  }

  disconnect() {
    this.connection.stop();
  }
}