import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';

import { NotificationsOverlayComponent } from './notifications-overlay/notifications-overlay.component';
import { NotificationsService } from './notifications.service';
import { SignalrService } from './signalr.service';
import { RfCodeService } from './rfcode.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,

    // PrimeNg
    ButtonModule,
    ToastModule,
  ],
  declarations: [
    NotificationsOverlayComponent
  ],
  providers: [
    MessageService,
    SignalrService,
    NotificationsService,
    RfCodeService,
  ],
  exports: [NotificationsOverlayComponent]
})
export class NotificationsModule { }
