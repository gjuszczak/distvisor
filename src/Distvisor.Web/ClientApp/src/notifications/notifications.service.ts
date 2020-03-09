import { MessageService } from 'primeng/api';
import { Injectable } from '@angular/core';

@Injectable()
export class NotificationsService {

  constructor(private messageService: MessageService) { }

  show(notification: INotification) {
    notification.show(this.messageService);
  }

  clear() {
    this.messageService.clear();
  }
}

interface INotification {
  show(messageService: MessageService): void;
}

export class SuccessNotification implements INotification {
  message: string;

  show(messageService: MessageService): void {
    messageService.add({
      severity: 'success',
      summary: 'Success',
      detail: this.message
    });
  }
}

export class ErrorNotification implements INotification {
  message: string;
  exceptionMessage: string;
  exceptionDetails: string;

  show(messageService: MessageService): void {
    messageService.add({
      severity: 'error',
      summary: this.message,
      detail: this.exceptionMessage
    });
  }
}

export class FakeApiUsedNotification implements INotification {
  api: string;
  requestParams: object;

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