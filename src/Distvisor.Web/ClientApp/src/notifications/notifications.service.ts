import { MessageService } from 'primeng/api';
import { Injectable } from '@angular/core';

@Injectable()
export class NotificationsService {

  constructor(private messageService: MessageService) { }

  success(detail: string) {
    this.messageService.add({ severity: 'success', summary: 'Success', detail });
  }

  error(summary: string, detail: string) {
    this.messageService.add({ severity: 'error', summary, detail });
  }

  addMultiple() {
    this.messageService.addAll([{ severity: 'success', summary: 'Service Message', detail: 'Via MessageService' },
    { severity: 'info', summary: 'Info Message', detail: 'Via MessageService' }]);
  }

  clear() {
    this.messageService.clear();
  }
}