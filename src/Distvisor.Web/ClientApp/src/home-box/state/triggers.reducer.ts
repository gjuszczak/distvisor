import { createReducer, on } from '@ngrx/store';
import * as TriggerActions from './triggers.actions';
import { HomeBoxTriggerDto } from 'src/api/models';

export const initialState: ReadonlyArray<HomeBoxTriggerDto> = [];

export const triggersReducer = createReducer(
  initialState,
  on(TriggerActions.triggersLoadedSuccess, (_, { triggers }) => 
    [...triggers]),
  on(TriggerActions.triggerAddedSuccess, (state, { trigger }) => 
    [...state, trigger]),
  on(TriggerActions.triggerDeletedSuccess, (state, { triggerId }) => 
    state.filter(x => x.id !== triggerId)),
  on(TriggerActions.triggerToggledSuccess, (state, { triggerId, enable }) => 
    state.map(x => x.id === triggerId ? { ...x, enabled: enable } : x)),
);