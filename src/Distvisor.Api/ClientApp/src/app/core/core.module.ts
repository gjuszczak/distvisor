import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ButtonModule } from 'primeng/button';
import { MenuModule } from 'primeng/menu';
import { RippleModule } from 'primeng/ripple';
import { SidebarModule } from 'primeng/sidebar';
import { SkeletonModule } from 'primeng/skeleton';
import { ToastModule } from 'primeng/toast';

import { CoreRoutingModule } from './core-routing.module';

import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { LogoutComponent } from './logout/logout.component';
import { PrivacyPolicyComponent } from './privacy-policy/privacy-policy.component';
import { SideNavComponent } from './side-nav/side-nav.component';
import { BrandComponent } from './brand/brand.component';
import { LayoutComponent } from './layout/layout.component';

@NgModule({
  imports: [
    CommonModule,
    CoreRoutingModule,

    // PrimeNg
    ButtonModule,
    MenuModule,
    RippleModule,
    SidebarModule,
    SkeletonModule,
    ToastModule,
  ],
  declarations: [
    BrandComponent,
    FooterComponent,
    HeaderComponent,
    HomeComponent,
    LayoutComponent,
    LogoutComponent,
    PrivacyPolicyComponent,
    SideNavComponent,
  ],
  exports: [
    LayoutComponent,
  ]
})
export class CoreModule { 
  constructor( @Optional() @SkipSelf() parentModule?: CoreModule) {
    if (parentModule) {
      throw new Error(`CoreModule has already been loaded. Import Core modules in the AppModule only.`);
   }
 }
}
