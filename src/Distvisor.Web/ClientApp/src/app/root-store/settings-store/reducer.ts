import { createReducer, on } from '@ngrx/store';
import * as HomeBoxActions from './actions';
import { HomeBoxSettingsState } from './state';

export const initialState: HomeBoxSettingsState = {
  gatewaySessions: {
    items: [],
    firstOffset: 0,
    pageSize: 5,
    pageSizeOptions: [5, 10, 25, 50],
    totalCount: 0,
    loading: false
  }
};

export const homeBoxSettingsReducer = createReducer(
  initialState,
  on(HomeBoxActions.loadGatewaySessions, (state) => ({
    ...state,
    gatewaySessions: {
      ...state.gatewaySessions,
      loading: true
    }
  })),
  on(HomeBoxActions.gatewaySessionsLoadedSuccess, (state, { gatewaySessions }) => ({
    ...state,
    gatewaySessions: {
      items: gatewaySessions.items ?? initialState.gatewaySessions.items,
      firstOffset: gatewaySessions.firstOffset ?? initialState.gatewaySessions.firstOffset,
      pageSize: gatewaySessions.pageSize ?? initialState.gatewaySessions.pageSize,
      totalCount: gatewaySessions.totalCount ?? initialState.gatewaySessions.totalCount,
      pageSizeOptions: initialState.gatewaySessions.pageSizeOptions,
      loading: false
    }
  })),
);