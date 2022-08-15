import { createAction, props } from '@ngrx/store';
import { DeleteGatewaySession, GatewaySessionDtoPaginatedList, LoginToGateway, RefreshGatewaySession } from 'src/app/api/models';

export const loadGatewaySessions = createAction(
  '[Home Box Settings] Load Gateway Sessions',
  props<{ firstOffset?: number, pageSize?: number }>()
);

export const gatewaySessionsLoadedSuccess = createAction(
  '[Home Box Settings] Gateway Sessions Loaded Success',
  props<{ gatewaySessions: GatewaySessionDtoPaginatedList }>()
);

export const loginToGateway = createAction(
  '[Home Box Settings] Login To Gateway',
  props<{ command: LoginToGateway }>()
);

export const refreshGatewaySession = createAction(
  '[Home Box Settings] Refresh Gateway Session',
  props<{ command: RefreshGatewaySession }>()
);

export const deleteGatewaySession = createAction(
  '[Home Box Settings] Delete Gateway Session',
  props<{ command: DeleteGatewaySession }>()
);