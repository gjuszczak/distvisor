import { createReducer, on } from '@ngrx/store';
import * as DevicesActions from './devices.actions';
import { HomeBoxDeviceDto } from 'src/api/models';

export const initialState: ReadonlyArray<HomeBoxDeviceDto> = [];

export const devicesReducer = createReducer(
  initialState,
  on(DevicesActions.devicesLoadedSuccess, (_, { devices }) =>
    [...devices]),
  on(DevicesActions.deviceUpdatedSuccess, (state, { device }) =>
    [...state.filter(d => d.id !== device.id), device]),
);