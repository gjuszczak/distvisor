import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { ApplicationPaths } from './auth.constants';
import { AutofocusModule } from '../autofocus/autofocus.module';
import { ApiModule } from '../api/api.module';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forChild(
      [
        { path: ApplicationPaths.Login, component: LoginComponent },
        { path: ApplicationPaths.LogOut, component: LogoutComponent },
        { path: ApplicationPaths.LoggedOut, component: LogoutComponent }
      ]
    ),

    // PrimeNg
    CardModule,
    ButtonModule,
    InputTextModule,
    PasswordModule,

    // internal
    AutofocusModule,
    ApiModule
  ],
  declarations: [LoginComponent, LogoutComponent],
  exports: [LoginComponent, LogoutComponent]
})
export class AuthModule { }
