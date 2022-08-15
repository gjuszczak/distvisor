import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { EMPTY } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as DialogActions from './dialogs.actions';
import * as TriggerActions from './triggers.actions';
import { HomeBoxService } from 'src/app/api/services';

@Injectable()
export class TriggersEffects {
    constructor(
        private actions$: Actions,
        private homeBoxService: HomeBoxService,
    ) { }

    loadTriggers$ = createEffect(() => this.actions$.pipe(
        ofType(TriggerActions.loadTriggers),
        mergeMap(() => 
            this.homeBoxService.apiSecHomeBoxTriggersGet$Json().pipe(
                map(triggers => TriggerActions.triggersLoadedSuccess({ triggers })),
                catchError(() => EMPTY)
            ))
    ));

    addTrigger$ = createEffect(() => this.actions$.pipe(
        ofType(TriggerActions.addTrigger),
        mergeMap(action => 
            this.homeBoxService.apiSecHomeBoxTriggersPost({
                body: action.trigger
            }).pipe(
                map(() => TriggerActions.triggerAddedSuccess({ trigger: action.trigger })),
                catchError(() => EMPTY)
            ))
    ));

    triggerAddedSuccess$ = createEffect(() => this.actions$.pipe(
        ofType(TriggerActions.triggerAddedSuccess),
        map(() => DialogActions.closeTriggerAddDialog() )
    ));

    deleteTrigger$ = createEffect(() => this.actions$.pipe(
        ofType(TriggerActions.deleteTrigger),
        mergeMap(action => 
            this.homeBoxService.apiSecHomeBoxTriggersIdDelete({
                id: action.triggerId
            }).pipe(
                map(() => TriggerActions.triggerDeletedSuccess({ triggerId: action.triggerId })),
                catchError(() => EMPTY)
            ))
    ));

    toggleTrigger$ = createEffect(() => this.actions$.pipe(
        ofType(TriggerActions.toggleTrigger),
        mergeMap(action => 
            this.homeBoxService.apiSecHomeBoxTriggersIdTogglePost({
                id: action.triggerId,
                enable: action.enable,
            }).pipe(
                map(() => TriggerActions.triggerToggledSuccess({ 
                    triggerId: action.triggerId,
                    enable: action.enable,
                })),
                catchError(() => EMPTY)
            ))
    ));

    executeTrigger$ = createEffect(() => this.actions$.pipe(
        ofType(TriggerActions.executeTrigger),
        mergeMap(action => 
            this.homeBoxService.apiSecHomeBoxTriggersIdExecutePost({
                id: action.triggerId,
            }).pipe(
                map(() => TriggerActions.triggerExecutedSuccess({ 
                    triggerId: action.triggerId,
                })),
                catchError(() => EMPTY)
            ))
    ));
}