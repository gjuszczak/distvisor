import { createReducer, on } from '@ngrx/store';
import { DialogsState } from './state';
import * as HomeBoxActions from './actions';

export const initialState: DialogsState = {
  isTriggerAddDialogOpened: false,
  isDeviceDetailsDialogOpened: false,
  deviceDetailsDialogParam: { deviceId: '' },
};

export const dialogsReducer = createReducer(
  initialState,
  on(HomeBoxActions.openTriggerAddDialog, (state) =>
    ({ ...state, isTriggerAddDialogOpened: true })),
  on(HomeBoxActions.closeTriggerAddDialog, (state) =>
    ({ ...state, isTriggerAddDialogOpened: false })),
  on(HomeBoxActions.openDeviceDetailsDialog, (state, { deviceId }) =>
    ({ ...state, isDeviceDetailsDialogOpened: true, deviceDetailsDialogParam: { deviceId } })),
  on(HomeBoxActions.closeDeviceDetailsDialog, (state) =>
    ({ ...state, isDeviceDetailsDialogOpened: false })),
);