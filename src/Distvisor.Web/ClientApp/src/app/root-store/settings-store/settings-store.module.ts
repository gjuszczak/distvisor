import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';

import { SettingsEffects } from './effects';
import { homeBoxSettingsReducer } from './reducer';

@NgModule({
  imports: [
    CommonModule,
    StoreModule.forFeature('settings', {
      homeBox: homeBoxSettingsReducer,
    }),
    EffectsModule.forFeature([
      SettingsEffects,
    ]),
  ],
  providers: [SettingsEffects]
})
export class SettingsStoreModule {}