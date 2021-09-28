import { createReducer, on } from '@ngrx/store';
import * as DialogActions from './dialogs.actions';
import { DialogsState } from './home-box.state';

export const initialState: DialogsState = {
  isTriggerAddDialogOpened: false,
};

export const dialogsReducer = createReducer(
  initialState,
  on(DialogActions.openTriggerAddDialog, (state) => 
    ({...state, isTriggerAddDialogOpened: true})),
  on(DialogActions.closeTriggerAddDialog, (state) => 
    ({...state, isTriggerAddDialogOpened: false})),
);