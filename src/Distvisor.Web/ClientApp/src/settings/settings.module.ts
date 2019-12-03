import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { FieldsetModule } from 'primeng/fieldset';
import { SettingsComponent } from './settings/settings.component';
import { AuthGuard } from '../auth/auth.guard';
import { NavigationModule } from '../navigation/navigation.module';
import { UpdatesComponent } from './updates/updates.component';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'settings', component: SettingsComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    ]),

    // internal
    NavigationModule,

    // PrimeNg
    FieldsetModule,
    ButtonModule,
    DropdownModule,
  ],
  declarations: [SettingsComponent, UpdatesComponent],
  exports: [SettingsComponent]
})
export class SettingsModule { }
