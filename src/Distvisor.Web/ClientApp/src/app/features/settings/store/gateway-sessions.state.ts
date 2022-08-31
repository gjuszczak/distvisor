import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { EMPTY } from "rxjs";
import { catchError, map } from 'rxjs/operators';

import { GatewaySessionDto } from 'src/app/api/models';
import { HomeBoxService } from 'src/app/api/services';
import { PaginatedList } from 'src/app/shared';

import { DeleteGatewaySession, LoadGatewaySessions, LoadGatewaySessionsSuccess, LoginToGateway, RefreshGatewaySession } from './gateway-sessions.actions';

export interface GatewaySessionsStateModel extends PaginatedList<GatewaySessionDto> {
}

export const gatewaySessionsStateDefaults: GatewaySessionsStateModel = {
    items: [],
    first: 0,
    rows: 5,
    rowsPerPageOptions: [5, 10, 25, 50],
    totalRecords: 0,
    loading: false,
    error: ''
};

@State<GatewaySessionsStateModel>({
    name: 'gatewaySessions',
    defaults: gatewaySessionsStateDefaults,
})
@Injectable()
export class GatewaySessionsState {
    constructor(private readonly homeBoxService: HomeBoxService) { }

    @Selector()
    static getGatewaySessions(state: GatewaySessionsStateModel) {
        return state;
    }

    @Action(LoadGatewaySessions)
    loadGatewaySessions({ dispatch, patchState }: StateContext<GatewaySessionsStateModel>, action: LoadGatewaySessions) {
        patchState({
            loading: true,
        });

        return this.homeBoxService.apiSHomeBoxGatewaySessionsGet$Json({
            first: action.firstOffset ?? gatewaySessionsStateDefaults.first,
            rows: action.pageSize ?? gatewaySessionsStateDefaults.rows
        }).pipe(
            map(gatewaySessions => dispatch(new LoadGatewaySessionsSuccess(gatewaySessions))),
            catchError(() => EMPTY)
        );
    }

    @Action(LoadGatewaySessionsSuccess)
    loadGatewaySessionsSuccess({ patchState }: StateContext<GatewaySessionsStateModel>, { gatewaySessions }: LoadGatewaySessionsSuccess) {
        patchState({
            items: gatewaySessions?.items ?? gatewaySessionsStateDefaults.items,
            first: gatewaySessions?.first ?? gatewaySessionsStateDefaults.first,
            rows: gatewaySessions?.rows ?? gatewaySessionsStateDefaults.rows,
            totalRecords: gatewaySessions?.totalRecords ?? gatewaySessionsStateDefaults.totalRecords,
            loading: false
        });
    }

    @Action(LoginToGateway)
    loginToGateway({ dispatch }: StateContext<GatewaySessionsStateModel>, { command }: LoginToGateway) {
        return this.homeBoxService.apiSHomeBoxLoginToGatewayPost({
            body: command,
        }).pipe(
            map(() => dispatch(new LoadGatewaySessions())),
            catchError(() => EMPTY)
        );
    }

    @Action(RefreshGatewaySession)
    refreshGatewaySession({ dispatch }: StateContext<GatewaySessionsStateModel>, { command }: RefreshGatewaySession) {
        return this.homeBoxService.apiSHomeBoxRefreshGatewaySessionPost({
            body: command,
        }).pipe(
            map(() => dispatch(new LoadGatewaySessions())),
            catchError(() => EMPTY)
        );
    }

    @Action(DeleteGatewaySession)
    deleteGatewaySession({ dispatch }: StateContext<GatewaySessionsStateModel>, { command }: DeleteGatewaySession) {
        return this.homeBoxService.apiSHomeBoxDeleteGatewaySessionPost({
            body: command,
        }).pipe(
            map(() => dispatch(new LoadGatewaySessions())),
            catchError(() => EMPTY)
        );
    }
}