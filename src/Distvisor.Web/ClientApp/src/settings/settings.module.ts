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
import { PasswordModule } from 'primeng/password';
import { RippleModule } from 'primeng/ripple';
import { SplitButtonModule } from 'primeng/splitbutton';
import { ApiModule } from '../api/api.module';
import { SettingsComponent } from './settings/settings.component';
import { DeploymentComponent } from './deployment/deployment.component';
import { SecretsVaultComponent } from './secrets-vault/secrets-vault.component';
import { RedirectionsComponent } from './redirections/redirections.component';
import { DatabasesComponent } from './databases/databases.component';
import { SharedModule } from 'src/shared';
import { HomeBoxSettingsComponent } from './home-box-settings/home-box-settings.component';
import { TableModule } from 'primeng/table';
import { StoreModule } from '@ngrx/store';
import { homeBoxReducer } from './state/home-box.reducer';
import { EffectsModule } from '@ngrx/effects';
import { HomeBoxEffects } from './state/home-box.effects';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'settings', component: SettingsComponent, pathMatch: 'full', canActivate: [MsalGuard] },
    ]),

    // NgRx
    StoreModule.forFeature('settings', {
      homeBox: homeBoxReducer,
    }),
    EffectsModule.forFeature([
      HomeBoxEffects,
    ]),
    
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
    SharedModule,
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
