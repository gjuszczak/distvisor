import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DropdownModule } from 'primeng/dropdown';
import { FieldsetModule } from 'primeng/fieldset';
import { AuthGuard } from '../auth/auth.guard';
import { SettingsComponent } from './settings/settings.component';
import { UpdatesComponent } from './updates/updates.component';


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'settings', component: SettingsComponent, pathMatch: 'full', canActivate: [AuthGuard] },
    ]),

    // PrimeNg
    FieldsetModule,
    ButtonModule,
    DropdownModule,
  ],
  declarations: [SettingsComponent, UpdatesComponent]
})
export class SettingsModule { }
