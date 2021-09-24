import { createReducer, on } from '@ngrx/store';
import * as TriggerActions from './triggers.actions';
import { HomeBoxTriggerDto } from 'src/api/models';

export const initialState: ReadonlyArray<HomeBoxTriggerDto> = [];

export const triggersReducer = createReducer(
  initialState,
  on(TriggerActions.triggersLoadedSuccess, (_, { triggers }) => [...triggers]),
);