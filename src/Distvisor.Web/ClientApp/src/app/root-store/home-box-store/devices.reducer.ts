import { createReducer, on } from '@ngrx/store';
import * as HomeBoxActions from './actions';
import { DeviceDto } from 'src/app/api/models';

export const initialState: ReadonlyArray<DeviceDto> = [];

export const devicesReducer = createReducer(
  initialState,
  on(HomeBoxActions.devicesLoadedSuccess, (_, { devices }) =>
    [...devices]),
);