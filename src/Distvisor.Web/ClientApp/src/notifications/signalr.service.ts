import { Injectable } from '@angular/core';
import { NotificationsService, SuccessNotification, ErrorNotification, FakeApiUsedNotification } from './notifications.service';

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
      console.log('Signalr notificationshub connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    this.connection.on("PushNotification", (payload: string) => {
      let notification = <GenericNotification>JSON.parse(payload);
      let notificationType = notification.$type;

      if (notificationType.includes('SuccessNotification')) {
        this.notificationsService.show(Object.assign(new SuccessNotification(), notification));
      }

      if (notificationType.includes('ErrorNotification')) {
        this.notificationsService.show(Object.assign(new ErrorNotification(), notification));
      }

      if (notificationType.includes('FakeApiUsedNotification')) {
        this.notificationsService.show(Object.assign(new FakeApiUsedNotification(), notification));
      }
    });
  }

  disconnect() {
    this.connection.stop();
  }
}

interface GenericNotification {
  $type: string;
}