import { createAction, props } from '@ngrx/store';
import { HomeBoxDeviceDto, UpdateHomeBoxDeviceDto } from 'src/api/models';

export const loadDevices = createAction(
  '[Home Box Devices] Load Devices'
);

export const devicesLoadedSuccess = createAction(
  '[Home Box Devices] Devices Loaded Success',
  props<{ devices: HomeBoxDeviceDto[] }>()
);

export const updateDevice = createAction(
  '[Home Box Devices] Update Device',
  props<{ device: UpdateHomeBoxDeviceDto }>()
);

export const deviceUpdatedSuccess = createAction(
  '[Home Box Devices] Device Updated Success',
  props<{ device: HomeBoxDeviceDto }>()
);