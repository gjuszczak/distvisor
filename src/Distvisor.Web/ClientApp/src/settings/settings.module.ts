import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { MsalGuard } from '@azure/msal-angular';
import { ButtonModule } from 'primeng/button';
import { DataViewModule } from 'primeng/dataview';
import { DropdownModule } from 'primeng/dropdown';
import { FieldsetModule } from 'primeng/fieldset';
import { InputTextModule } from 'primeng/inputtext';
import { PanelModule } from 'primeng/panel';
import { SplitButtonModule } from 'primeng/splitbutton';
import { ApiModule } from '../api/api.module';
import { SettingsComponent } from './settings/settings.component';
import { DeploymentComponent } from './deployment/deployment.component';
import { MicrosoftAuthComponent } from './microsoft-auth/microsoft-auth.component';
import { SecretsVaultComponent } from './secrets-vault/secrets-vault.component';
import { RedirectionsComponent } from './redirections/redirections.component';
import { DatabasesComponent } from './databases/databases.component';


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
    DataViewModule,
    DropdownModule,
    FieldsetModule,
    InputTextModule,
    PanelModule,
    SplitButtonModule,

    // internal
    ApiModule,
  ],
  declarations: [
    SettingsComponent,
    DeploymentComponent,
    MicrosoftAuthComponent,
    DatabasesComponent,
    SecretsVaultComponent,
    RedirectionsComponent,
  ]
})
export class SettingsModule { }
