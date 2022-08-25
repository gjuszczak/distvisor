import { createFeatureSelector, createSelector } from '@ngrx/store';
import { State } from './state';

export const selectSettingsState = createFeatureSelector<State>('settings');

export const selectHomeBoxSettings = createSelector(
  selectSettingsState,
  state => state.homeBox
);

export const selectGatewaySessions = createSelector(
  selectHomeBoxSettings,
  homeBoxSettings => homeBoxSettings.gatewaySessions
);