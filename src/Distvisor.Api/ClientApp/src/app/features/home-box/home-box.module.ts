import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgxsModule } from '@ngxs/store';

import { ConfirmationService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { CodeHighlighterModule } from 'primeng/codehighlighter';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { DataViewModule } from 'primeng/dataview';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { InplaceModule } from 'primeng/inplace';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { MenuModule } from 'primeng/menu';
import { PasswordModule } from 'primeng/password';
import { RippleModule } from 'primeng/ripple';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';

import { HomeBoxState } from './store/home-box.state';
import { GatewaySessionsState } from './store/gateway-sessions.state';

import { ApiModule } from 'src/app/api/api.module';
import { SharedModule } from 'src/app/shared';
import { HomeBoxRoutingModule } from './home-box-routing.module';

import { DevicesComponent } from './devices/devices.component';
import { GatewaySessionsComponent } from './gateway-sessions/gateway-sessions.component';
import { DevicesState } from './store/devices.state';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,

    // Ngxs
    NgxsModule.forFeature([HomeBoxState, GatewaySessionsState, DevicesState]),

    // PrimeNg
    ButtonModule,
    CardModule,
    CodeHighlighterModule,
    ConfirmPopupModule,
    DataViewModule,
    DialogModule,
    DropdownModule,
    InplaceModule,
    InputNumberModule,
    InputSwitchModule,
    InputTextareaModule,
    InputTextModule,
    MenuModule,
    PasswordModule,
    RippleModule,
    TableModule,
    TabViewModule,

    // internal
    ApiModule,
    SharedModule,
    HomeBoxRoutingModule,
  ],
  declarations: [
    DevicesComponent,
    GatewaySessionsComponent,
  ],
  providers: [ConfirmationService]
})
export class HomeBoxModule { }
