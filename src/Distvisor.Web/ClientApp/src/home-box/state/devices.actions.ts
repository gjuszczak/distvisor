import { createAction, props } from '@ngrx/store';
import { DeviceDto } from 'src/api/models';

export const loadDevices = createAction(
  '[Home Box Devices] Load Devices'
);

export const devicesLoadedSuccess = createAction(
  '[Home Box Devices] Devices Loaded Success',
  props<{ devices: DeviceDto[] }>()
);