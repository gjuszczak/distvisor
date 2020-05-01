import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';

import { NotificationsPageComponent } from './notifications-page/notifications-page.component';
import { NotificationsOverlayComponent } from './notifications-overlay/notifications-overlay.component';
import { NotificationsService } from './notifications.service';
import { SignalrService } from './signalr.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'notifications', component: NotificationsPageComponent, pathMatch: 'full', canActivate: [MsalGuard] },
    ]),

    // PrimeNg
    ButtonModule,
    ToastModule,
  ],
  declarations: [
    NotificationsPageComponent,
    NotificationsOverlayComponent
  ],
  providers: [
    MessageService,
    SignalrService,
    NotificationsService
  ],
  exports: [NotificationsOverlayComponent]
})
export class NotificationsModule { }
