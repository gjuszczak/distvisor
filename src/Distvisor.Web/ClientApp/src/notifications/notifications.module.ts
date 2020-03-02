import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';

import { AuthGuard } from '../auth/auth.guard';
import { NotificationsPageComponent } from './notifications-page/notifications-page.component';
import { NotificationsOverlayComponent } from './notifications-overlay/notifications-overlay.component';
import { NotificationsService } from './notifications.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'notifications', component: NotificationsPageComponent, pathMatch: 'full', canActivate: [AuthGuard] },
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
    NotificationsService
  ],
  exports: [NotificationsOverlayComponent]
})
export class NotificationsModule { }
