import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MenuModule } from 'primeng/menu';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';

import { CoreRoutingModule } from './core-routing.module';

import { HomeComponent } from './home/home.component';
import { LogoutComponent } from './logout/logout.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { FooterComponent } from './footer/footer.component';
import { PrivacyPolicyComponent } from './privacy-policy/privacy-policy.component';

@NgModule({
  imports: [
    CommonModule,
    CoreRoutingModule,

    // PrimeNg
    MenuModule,
    ButtonModule,
    RippleModule,
    ToastModule,
  ],
  declarations: [
    HomeComponent,
    LogoutComponent,
    NavMenuComponent,
    FooterComponent,
    PrivacyPolicyComponent
  ],
  exports: [
    NavMenuComponent
  ]
})
export class CoreModule { 
  constructor( @Optional() @SkipSelf() parentModule?: CoreModule) {
    if (parentModule) {
      throw new Error(`CoreModule has already been loaded. Import Core modules in the AppModule only.`);
   }
 }
}
