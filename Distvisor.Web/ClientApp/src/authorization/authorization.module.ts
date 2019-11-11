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
import { LoginMenuComponent } from './login-menu/login-menu.component';
import { LogoutComponent } from './logout/logout.component';
import { ApplicationPaths } from './authorization.constants';
import { AutofocusModule } from '../autofocus/autofocus.module';

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
    AutofocusModule
  ],
  declarations: [LoginMenuComponent, LoginComponent, LogoutComponent],
  exports: [LoginMenuComponent, LoginComponent, LogoutComponent]
})
export class AuthorizationModule { }
