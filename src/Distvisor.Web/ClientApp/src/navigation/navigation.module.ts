import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuModule } from 'primeng/menu';
import { ButtonModule } from 'primeng/button';
import { NavigationService } from './navigation.service';
import { NavMenuComponent } from './nav-menu/nav-menu.component';

@NgModule({
  imports: [
    CommonModule,

    // prime-ng
    MenuModule,
    ButtonModule,
  ],
  declarations: [NavMenuComponent],
  providers: [NavigationService],
  exports: [NavMenuComponent]
})
export class NavigationModule { }
