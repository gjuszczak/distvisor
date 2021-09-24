import { createReducer } from '@ngrx/store';
import { DialogsState } from './home-box.state';

export const initialState: DialogsState = {
  isTriggerAddDialogOpened: false,
};

export const dialogsReducer = createReducer(
  initialState
);