import { CommonModule } from '@angular/common';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

import { SettingsStoreModule } from './settings-store';
import { EventLogStoreModule } from './event-log-store';
import { HomeBoxStoreModule } from './home-box-store';

@NgModule({
  imports: [
    CommonModule,
    SettingsStoreModule,
    EventLogStoreModule,
    HomeBoxStoreModule,
    StoreModule.forRoot({}),
    EffectsModule.forRoot([]),
    StoreDevtoolsModule.instrument({
      maxAge: 25, // Retains last 25 states
    }),
  ],
  declarations: []
})
export class RootStoreModule {
  constructor(@Optional() @SkipSelf() parentModule?: RootStoreModule) {
    if (parentModule) {
      throw new Error(`RootStoreModule has already been loaded. Import Core modules in the AppModule only.`);
    }
  }
}