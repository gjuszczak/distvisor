import { createAction, props } from '@ngrx/store';
import { DeviceDto } from 'src/app/api/models';

export const loadDevices = createAction(
  '[Home Box Devices] Load Devices'
);

export const devicesLoadedSuccess = createAction(
  '[Home Box Devices] Devices Loaded Success',
  props<{ devices: DeviceDto[] }>()
);

export const openTriggerAddDialog = createAction(
  '[Home Box Dialogs] Open Trigger Add dialog'
);

export const closeTriggerAddDialog = createAction(
  '[Home Box Dialogs] Close Trigger Add dialog'
);

export const openDeviceDetailsDialog = createAction(
  '[Home Box Dialogs] Open Device Details dialog',
  props<{ deviceId: string }>()
);

export const closeDeviceDetailsDialog = createAction(
  '[Home Box Dialogs] Close Device Details dialog'
);