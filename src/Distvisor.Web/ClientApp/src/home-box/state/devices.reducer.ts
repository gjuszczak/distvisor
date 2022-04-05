import { createReducer, on } from '@ngrx/store';
import * as DevicesActions from './devices.actions';
import { DeviceDto } from 'src/api/models';

export const initialState: ReadonlyArray<DeviceDto> = [];

export const devicesReducer = createReducer(
  initialState,
  on(DevicesActions.devicesLoadedSuccess, (_, { devices }) =>
    [...devices]),
);