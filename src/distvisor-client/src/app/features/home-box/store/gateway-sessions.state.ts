import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { patch } from '@ngxs/store/operators';
import { Observable } from "rxjs";
import { catchError, mergeMap } from 'rxjs/operators';

import { HomeBoxService } from 'src/app/api/services';
import { GatewaySessionDto, GatewaySessionStatus } from 'src/app/api/models';
import { PaginatedList } from 'src/app/shared';

import { DeleteGatewaySession, LoadGatewaySessions, LoadGatewaySessionsFail, LoadGatewaySessionsSuccess, OpenGatewaySession, RefreshGatewaySession } from './gateway-sessions.actions';

export interface GatewaySession {
    id: string;
    status: GatewaySessionStatus;
    tokenGeneratedAt: string;
}

export interface GatewaySessionsList extends PaginatedList<GatewaySession> {
};

export interface GatewaySessionsStateModel {
    list: GatewaySessionsList,
};

export const gatewaySessionsStateDefaults: GatewaySessionsStateModel = {
    list: {
        items: [],
        first: 0,
        rows: 5,
        rowsPerPageOptions: [5, 10, 25, 50],
        totalRecords: 0,
        loading: false,
        error: ''
    }
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
        return state.list;
    }

    @Action(LoadGatewaySessions)
    loadGatewaySessions(ctx: StateContext<GatewaySessionsStateModel>, action: LoadGatewaySessions) {
        const state = ctx.getState();

        const first = (
            action.first
            && !isNaN(Number(action.first)))
            ? Number(action.first)
            : state.list.first;

        const rows = (
            action.rows
            && !isNaN(Number(action.rows))
            && state.list.rowsPerPageOptions.indexOf(Number(action.rows)) >= 0)
            ? Number(action.rows)
            : state.list.rows;

        ctx.setState(this.startListLoading());

        return this.homeBoxService.apiHomeBoxSessionsGet$Json({ first, rows }).pipe(
            mergeMap(gatewaySessions => ctx.dispatch(new LoadGatewaySessionsSuccess(gatewaySessions))),
            catchError(e => ctx.dispatch(new LoadGatewaySessionsFail(e.error.title)))
        );
    }

    @Action(LoadGatewaySessionsSuccess)
    loadGatewaySessionsSuccess(ctx: StateContext<GatewaySessionsStateModel>, { gatewaySessions }: LoadGatewaySessionsSuccess) {
        ctx.setState(patch<GatewaySessionsStateModel>({
            list: patch<GatewaySessionsList>({
                items: this.dtosToGatewaySessions(gatewaySessions?.items ?? []),
                first: gatewaySessions?.first ?? undefined,
                rows: gatewaySessions?.rows ?? undefined,
                totalRecords: gatewaySessions?.totalRecords ?? undefined,
                loading: false,
                error: ''
            })
        }));
    }

    @Action(LoadGatewaySessionsFail)
    loadGatewaySessionsFail(ctx: StateContext<GatewaySessionsStateModel>, { error }: LoadGatewaySessionsFail) {
        ctx.setState(this.failListLoading(error));
    }

    @Action(OpenGatewaySession)
    loginToGateway(ctx: StateContext<GatewaySessionsStateModel>, { user, password }: OpenGatewaySession) {
        ctx.setState(this.startListLoading());

        return this.homeBoxService.apiHomeBoxSessionsOpenPost({ body: { user, password } }).pipe(
            this.handleFetchResponse(ctx)
        );
    }

    @Action(RefreshGatewaySession)
    refreshGatewaySession(ctx: StateContext<GatewaySessionsStateModel>, { sessionId }: RefreshGatewaySession) {
        return this.homeBoxService.apiHomeBoxSessionsRefreshPost({ body: { sessionId } }).pipe(
            this.handleFetchResponse(ctx)
        );
    }

    @Action(DeleteGatewaySession)
    deleteGatewaySession(ctx: StateContext<GatewaySessionsStateModel>, { sessionId }: DeleteGatewaySession) {
        return this.homeBoxService.apiHomeBoxSessionsDeletePost({ body: { sessionId } }).pipe(
            this.handleFetchResponse(ctx)
        );
    }

    private startListLoading() {
        return patch<GatewaySessionsStateModel>({
            list: patch<GatewaySessionsList>({
                loading: true,
                error: '',
            })
        });
    }

    private failListLoading(error: string) {
        return patch<GatewaySessionsStateModel>({
            list: patch<GatewaySessionsList>({
                items: [],
                loading: false,
                error: error ? error : "Unexpected error",
            })
        });
    }

    private handleFetchResponse<T>(ctx: StateContext<GatewaySessionsStateModel>) {
        return (source: Observable<T>) =>
            source.pipe(
                mergeMap(() => ctx.dispatch(new LoadGatewaySessions())),
                catchError((e: any) => ctx.dispatch(new LoadGatewaySessionsFail(e.error.title)))
            );
    };

    private dtosToGatewaySessions(dtos: GatewaySessionDto[]) {
        const gatewaySessions: GatewaySession[] = dtos
            .filter(dto => dto.id && dto.status && dto.tokenGeneratedAt)
            .map(dto => <GatewaySession>{
                id: dto.id,
                status: dto.status,
                tokenGeneratedAt: dto.tokenGeneratedAt
            });

        const invalidCount = dtos.length - gatewaySessions.length;
        if (invalidCount > 0) {
            console.error(`${invalidCount} invalid GatewaySessionDto received.`);
        }

        return gatewaySessions;
    }
}