import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';

import { devicesReducer } from './devices.reducer';
import { dialogsReducer } from './dialogs.reducer';
import { HomeBoxEffects } from './effects';

@NgModule({
  imports: [
    CommonModule,
    StoreModule.forFeature('homeBox', {
      devices: devicesReducer,
      dialogs: dialogsReducer,
    }),
    EffectsModule.forFeature([
      HomeBoxEffects,
    ]),
  ],
  providers: [HomeBoxEffects]
})
export class HomeBoxStoreModule {}