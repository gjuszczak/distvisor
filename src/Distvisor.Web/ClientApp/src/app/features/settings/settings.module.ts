import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { NgxsModule } from '@ngxs/store';

import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DataViewModule } from 'primeng/dataview';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { MenuModule } from 'primeng/menu';
import { PanelModule } from 'primeng/panel';
import { PasswordModule } from 'primeng/password';
import { RippleModule } from 'primeng/ripple';
import { SplitButtonModule } from 'primeng/splitbutton';
import { TableModule } from 'primeng/table';

import { ApiModule } from 'src/app/api/api.module';
import { RootStoreModule } from 'src/app/root-store';
import { SharedModule } from 'src/app/shared';
import { SettingsRoutingModule } from './settings-routing.module';

import { HomeBoxSettingsState } from './store/home-box-settings.state';
import { GatewaySessionsState } from './store/gateway-sessions.state';

import { SettingsComponent } from './settings/settings.component';
import { DeploymentComponent } from './deployment/deployment.component';
import { SecretsVaultComponent } from './secrets-vault/secrets-vault.component';
import { RedirectionsComponent } from './redirections/redirections.component';
import { DatabasesComponent } from './databases/databases.component';
import { HomeBoxSettingsComponent } from './home-box-settings/home-box-settings.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    
    // Ngxs
    NgxsModule.forFeature([HomeBoxSettingsState, GatewaySessionsState]),

    // PrimeNg
    ButtonModule,
    CardModule,
    DataViewModule,
    DropdownModule,
    InputTextModule,
    MenuModule,
    PanelModule,
    PasswordModule,
    RippleModule,
    SplitButtonModule,
    TableModule,

    // internal
    ApiModule,
    RootStoreModule,
    SharedModule,
    SettingsRoutingModule,
  ],
  declarations: [
    SettingsComponent,
    DeploymentComponent,
    DatabasesComponent,
    SecretsVaultComponent,
    RedirectionsComponent,
    HomeBoxSettingsComponent,
  ]
})
export class SettingsModule { }
