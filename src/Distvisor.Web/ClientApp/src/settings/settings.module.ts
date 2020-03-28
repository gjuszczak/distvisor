import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DataViewModule } from 'primeng/dataview';
import { DropdownModule } from 'primeng/dropdown';
import { FieldsetModule } from 'primeng/fieldset';
import { InputTextModule } from 'primeng/inputtext';
import { ApiModule } from '../api/api.module';
import { AuthGuard } from '../auth/auth.guard';
import { SettingsComponent } from './settings/settings.component';
import { UpdatesComponent } from './updates/updates.component';
import { MicrosoftAuthComponent } from './microsoft-auth/microsoft-auth.component';
import { SecretsVaultComponent } from './secrets-vault/secrets-vault.component';
import { RedirectionsComponent } from './redirections/redirections.component';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'settings', component: SettingsComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    ]),

    // PrimeNg
    ButtonModule,
    DataViewModule,
    DropdownModule,
    FieldsetModule,
    InputTextModule,

    // internal
    ApiModule,
  ],
  declarations: [
    SettingsComponent,
    UpdatesComponent,
    MicrosoftAuthComponent,
    SecretsVaultComponent,
    RedirectionsComponent,
  ]
})
export class SettingsModule { }
