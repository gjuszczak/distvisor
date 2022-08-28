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
    firstOffset: 0,
    pageSize: 5,
    pageSizeOptions: [5, 10, 25, 50],
    totalCount: 0,
    loading: false
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
            firstOffset: action.firstOffset ?? gatewaySessionsStateDefaults.firstOffset,
            pageSize: action.pageSize ?? gatewaySessionsStateDefaults.pageSize
        }).pipe(
            map(gatewaySessions => dispatch(new LoadGatewaySessionsSuccess(gatewaySessions))),
            catchError(() => EMPTY)
        );
    }

    @Action(LoadGatewaySessionsSuccess)
    loadGatewaySessionsSuccess({ patchState }: StateContext<GatewaySessionsStateModel>, { gatewaySessions }: LoadGatewaySessionsSuccess) {
        patchState({
            items: gatewaySessions?.items ?? gatewaySessionsStateDefaults.items,
            firstOffset: gatewaySessions?.firstOffset ?? gatewaySessionsStateDefaults.firstOffset,
            pageSize: gatewaySessions?.pageSize ?? gatewaySessionsStateDefaults.pageSize,
            totalCount: gatewaySessions?.totalCount ?? gatewaySessionsStateDefaults.totalCount,
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