import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { EMPTY } from 'rxjs';
import { catchError, map, mergeMap, withLatestFrom } from 'rxjs/operators';
import * as HomeBoxActions from './home-box.actions';
import { SettingsState } from './settings.state';
import { selectGatewaySessions } from './home-box.selectors';
import { HomeBoxService } from 'src/api/services';

@Injectable()
export class HomeBoxEffects {
    constructor(
        private actions$: Actions,
        private homeBoxService: HomeBoxService,
        private store: Store<SettingsState>
    ) { }

    loadGatewaySessions$ = createEffect(() => this.actions$.pipe(
        ofType(HomeBoxActions.loadGatewaySessions),
        withLatestFrom(this.store.select(selectGatewaySessions)),
        mergeMap(([action, gatewaySessions]) =>
            this.homeBoxService.apiSHomeBoxGatewaySessionsGet$Json({
                firstOffset: action.firstOffset ?? gatewaySessions.firstOffset,
                pageSize: action.pageSize ?? gatewaySessions.pageSize
            }).pipe(
                map(gatewaySessions => HomeBoxActions.gatewaySessionsLoadedSuccess({ gatewaySessions })),
                catchError(() => EMPTY)
            ))
    ));

    loginToGateway$ = createEffect(() => this.actions$.pipe(
        ofType(HomeBoxActions.loginToGateway),
        mergeMap(action =>
            this.homeBoxService.apiSHomeBoxLoginToGatewayPost({
                body: action.command,
            }).pipe(
                map(() => HomeBoxActions.loadGatewaySessions({})),
                catchError(() => EMPTY)
            ))
    ));

    refreshGatewaySession$ = createEffect(() => this.actions$.pipe(
        ofType(HomeBoxActions.refreshGatewaySession),
        mergeMap(action =>
            this.homeBoxService.apiSHomeBoxRefreshGatewaySessionPost({
                body: action.command,
            }).pipe(
                map(() => HomeBoxActions.loadGatewaySessions({})),
                catchError(() => EMPTY)
            ))
    ));

    deleteGatewaySession$ = createEffect(() => this.actions$.pipe(
        ofType(HomeBoxActions.deleteGatewaySession),
        mergeMap(action =>
            this.homeBoxService.apiSHomeBoxDeleteGatewaySessionPost({
                body: action.command,
            }).pipe(
                map(() => HomeBoxActions.loadGatewaySessions({})),
                catchError(() => EMPTY)
            ))
    ));
}