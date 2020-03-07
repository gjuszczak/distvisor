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
      let notification: any = JSON.parse(payload);
      let notificationType = (<GenericNotification>notification).$type;

      if (notificationType.includes('SuccessNotification')) {
        this.successNotification(<SuccessNotification>notification);
      }

      if (notificationType.includes('ErrorNotification')) {
        this.errorNotification(<ErrorNotification>notification);
      }

    });
  }

  disconnect() {
    this.connection.stop();
  }

  successNotification(notification: SuccessNotification) {
    this.notificationsService.success(notification.message);
  }

  errorNotification(notification: ErrorNotification) {
    this.notificationsService.error(notification.message, notification.exceptionMessage);
  }
}

interface GenericNotification {
  $type: string;
}

interface SuccessNotification {
  message: string;
}

interface ErrorNotification {
  message: string;
  exceptionMessage: string;
  exceptionDetails: string;
}