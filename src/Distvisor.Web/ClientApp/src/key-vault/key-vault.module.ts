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
import { KeyVaultComponent } from './key-vault/key-vault.component';
import { KeyVaultIfirmaComponent } from './key-vault-ifirma/key-vault-ifirma.component';
import { KeyVaultGithubComponent } from './key-vault-github/key-vault-github.component';
import { KeyVaultMailgunComponent } from './key-vault-mailgun/key-vault-mailgun.component';

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
    KeyVaultComponent, 
    KeyVaultGithubComponent,
    KeyVaultIfirmaComponent,
    KeyVaultMailgunComponent,
  ],
  exports: [KeyVaultComponent],
})
export class KeyVaultModule { }
