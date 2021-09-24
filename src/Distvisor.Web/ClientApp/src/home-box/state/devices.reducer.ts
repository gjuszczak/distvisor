import { createReducer } from '@ngrx/store';
import { HomeBoxDeviceDto } from 'src/api/models';

export const initialState: ReadonlyArray<HomeBoxDeviceDto> = [];

export const devicesReducer = createReducer(
  initialState
);