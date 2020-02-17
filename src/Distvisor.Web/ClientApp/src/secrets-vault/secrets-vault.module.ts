import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { ButtonModule } from 'primeng/button';
import { DataViewModule } from 'primeng/dataview';
import { DropdownModule } from 'primeng/dropdown';
import { FieldsetModule } from 'primeng/fieldset';
import { InputTextModule } from 'primeng/inputtext';

import { ApiModule } from '../api/api.module';
import { SecretsVaultComponent } from './secrets-vault/secrets-vault.component';
import { SecretValueComponent } from './secret-value/secret-value.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,

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
    SecretsVaultComponent, 
    SecretValueComponent,
  ],
  exports: [SecretsVaultComponent],
})
export class SecretsVaultModule { }
