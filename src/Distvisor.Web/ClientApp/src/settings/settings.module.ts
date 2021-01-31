import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { MsalGuard } from '@azure/msal-angular';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { DataViewModule } from 'primeng/dataview';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { MenuModule } from 'primeng/menu';
import { PanelModule } from 'primeng/panel';
import { RippleModule } from 'primeng/ripple';
import { SplitButtonModule } from 'primeng/splitbutton';
import { ApiModule } from '../api/api.module';
import { SettingsComponent } from './settings/settings.component';
import { DeploymentComponent } from './deployment/deployment.component';
import { SecretsVaultComponent } from './secrets-vault/secrets-vault.component';
import { RedirectionsComponent } from './redirections/redirections.component';
import { DatabasesComponent } from './databases/databases.component';
import { UtilsModule } from 'src/utils/utils.module';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'settings', component: SettingsComponent, pathMatch: 'full', canActivate: [MsalGuard] },
    ]),

    // PrimeNg
    ButtonModule,
    CardModule,
    DataViewModule,
    DropdownModule,
    InputTextModule,
    MenuModule,
    PanelModule,
    RippleModule,
    SplitButtonModule,

    // internal
    ApiModule,
    UtilsModule,
  ],
  declarations: [
    SettingsComponent,
    DeploymentComponent,
    DatabasesComponent,
    SecretsVaultComponent,
    RedirectionsComponent,
  ]
})
export class SettingsModule { }
