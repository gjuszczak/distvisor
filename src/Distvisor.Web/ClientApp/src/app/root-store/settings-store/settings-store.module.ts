import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';

import { SettingsEffects } from './effects';
import { homeBoxReducer } from './reducer';

@NgModule({
  imports: [
    CommonModule,
    StoreModule.forFeature('settings', {
      homeBox: homeBoxReducer,
    }),
    EffectsModule.forFeature([
      SettingsEffects,
    ]),
  ],
  providers: [SettingsEffects]
})
export class SettingsStoreModule {}