import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SettingsComponent } from './settings/settings.component';
import { AuthorizeGuard } from '../authorization/authorize.guard';
import { NavigationModule } from '../navigation/navigation.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot([
      { path: 'settings', component: SettingsComponent, pathMatch: 'full', canActivate: [AuthorizeGuard] },
    ]),

    // internal
    NavigationModule,
  ],
  declarations: [SettingsComponent],
  exports: [SettingsComponent]
})
export class SettingsModule { }
