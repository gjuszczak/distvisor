import { createReducer, on } from '@ngrx/store';
import * as HomeBoxActions from './home-box.actions';
import { HomeBoxSettingsState } from './settings.state';

export const initialState: HomeBoxSettingsState = {
  sessions: []
};

export const homeBoxReducer = createReducer(
  initialState,
  on(HomeBoxActions.sessionsLoadedSuccess, (state, { sessions }) =>
    ({ ...state, sessions })),
);