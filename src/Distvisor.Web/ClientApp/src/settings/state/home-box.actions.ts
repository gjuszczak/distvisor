import { createAction, props } from '@ngrx/store';

export const loadHomeBoxSessions = createAction(
  '[Home Box Settings] Load Sessions'
);

export const sessionsLoadedSuccess = createAction(
  '[Home Box Settings] Sessions Loaded Success',
  props<{ sessions: string[] }>()
);