import { createReducer, on } from '@ngrx/store';
import { DialogsState } from './home-box.state';
import * as DialogActions from './dialogs.actions';

export const initialState: DialogsState = {
  isTriggerAddDialogOpened: false,
  isDeviceDetailsDialogOpened: false,
  deviceDetailsDialogParam: { deviceId: '' },
};

export const dialogsReducer = createReducer(
  initialState,
  on(DialogActions.openTriggerAddDialog, (state) =>
    ({ ...state, isTriggerAddDialogOpened: true })),
  on(DialogActions.closeTriggerAddDialog, (state) =>
    ({ ...state, isTriggerAddDialogOpened: false })),
  on(DialogActions.openDeviceDetailsDialog, (state, { deviceId }) =>
    ({ ...state, isDeviceDetailsDialogOpened: true, deviceDetailsDialogParam: { deviceId } })),
  on(DialogActions.closeDeviceDetailsDialog, (state) =>
    ({ ...state, isDeviceDetailsDialogOpened: false })),
);