import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

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
import { RippleModule } from 'primeng/ripple';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';

import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { TriggersEffects } from './state/triggers.effects';
import { DevicesEffects } from './state/devices.effects';
import { devicesReducer } from './state/devices.reducer';
import { triggersReducer } from './state/triggers.reducer';
import { dialogsReducer } from './state/dialogs.reducer';

import { ApiModule } from '../api/api.module';
import { HomeBoxComponent } from './home-box/home-box.component';
import { TriggersListComponent } from './triggers-list/triggers-list.component';
import { DevicesListComponent } from './devices-list/devices-list.component';
import { DeviceDetailsDialogComponent } from './device-details-dialog/device-details-dialog.component';
import { TriggerAddDialogComponent } from './trigger-add-dialog/trigger-add-dialog.component';
import { SharedModule } from 'src/app-common';
import { SignalrModule } from 'src/signalr/signalr.module';

@NgModule({
  imports: [
    ButtonModule,
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'home-box', component: HomeBoxComponent, pathMatch: 'full', canActivate: [MsalGuard] },
    ]),

    // NgRx
    StoreModule.forFeature('homeBox', {
      devices: devicesReducer,
      triggers: triggersReducer,
      dialogs: dialogsReducer,
    }),
    EffectsModule.forFeature([
      TriggersEffects,
      DevicesEffects,
    ]),

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
    RippleModule,
    TableModule,
    TabViewModule,

    // internal
    ApiModule,
    SharedModule,
    SignalrModule
  ],
  declarations: [
    HomeBoxComponent,
    DevicesListComponent,
    TriggersListComponent,
    TriggerAddDialogComponent,
    DeviceDetailsDialogComponent,
  ],
  providers: [ConfirmationService]
})
export class HomeBoxModule { }
