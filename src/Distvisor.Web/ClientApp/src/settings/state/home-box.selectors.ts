import { createSelector } from '@ngrx/store';
import { SettingsState } from './settings.state';


export const selectHomeBox = (state: SettingsState) => state.settings.homeBox;

export const selectGatewaySessions = createSelector(
  selectHomeBox,
  homeBox => homeBox.gatewaySessions
);