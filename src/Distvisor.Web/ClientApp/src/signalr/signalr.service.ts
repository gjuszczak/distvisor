import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { RfCodeService } from './rfcode.service';

import * as signalR from '@microsoft/signalr';

@Injectable()
export class SignalrService {

  private connection: signalR.HubConnection | null;

  constructor(
    private messageService: MessageService, 
    private rfCodeService: RfCodeService) {
    this.connection = null;
  }

  connect(baseUrl: string, userSession: string) {
    const baseUrlClean = baseUrl.replace(/\/$/, "");
    this.connection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Information)
      .withUrl(`${baseUrlClean}/hubs/notificationshub`, { accessTokenFactory: () => userSession })
      .withAutomaticReconnect()
      .build();

    this.connection.start().then(function () {
      console.log('Signalr notificationshub connected!');
    }).catch(function (err) {
      return console.error(err.toString());
    });

    this.connection.on("PushNotification", (payload: string) => {
      let notification = <GenericNotification>JSON.parse(payload);
      let notificationType = notification.typeName;

      if (notificationType.includes('SuccessNotification')) {
        this.showNotification(Object.assign(new SuccessNotification(), notification));
      }

      if (notificationType.includes('ErrorNotification')) {
        this.showNotification(Object.assign(new ErrorNotification(), notification));
      }

      if (notificationType.includes('FakeApiUsedNotification')) {
        this.showNotification(Object.assign(new FakeApiUsedNotification(), notification));
      }
    });

    this.connection.on("PushRfCode", (code: string) => {
      this.rfCodeService.rfCodeSubject$.next(code);
    });
  }

  disconnect() {
    if (this.connection) {
      this.connection.stop();
      this.connection = null;
    }
  }

  private showNotification(notification: INotification) {
    notification.show(this.messageService);
  }
}

interface GenericNotification {
  typeName: string;
}

interface INotification {
  show(messageService: MessageService): void;
}

class SuccessNotification implements INotification {
  message: string = 'unknown_message';

  show(messageService: MessageService): void {
    messageService.add({
      severity: 'success',
      summary: 'Success',
      detail: this.message
    });
  }
}

class ErrorNotification implements INotification {
  message: string = 'unknown_message';
  exceptionMessage: string = 'unknown_excepiton_message';
  exceptionDetails: string = 'unknown_exception_details';

  show(messageService: MessageService): void {
    messageService.add({
      severity: 'error',
      summary: this.message,
      detail: this.exceptionMessage
    });
  }
}

class FakeApiUsedNotification implements INotification {
  api: string = 'unknown_api';
  requestParams: any = 'unknown_params';

  show(messageService: MessageService): void {
    if(this.requestParams['$type']){
      this.requestParams['$type'] = undefined;
    }

    messageService.add({
      severity: 'warn',
      summary: `Fake ${this.api} api called. Remote requests are currently disabled on dev env.`,
      detail: JSON.stringify(this.requestParams, null, 2)
    });
  }
}