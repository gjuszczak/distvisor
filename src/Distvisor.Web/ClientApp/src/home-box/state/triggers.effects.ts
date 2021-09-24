import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { EMPTY } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as TriggerActions from './triggers.actions';
import { HomeBoxService } from 'src/api/services';

@Injectable()
export class TriggersEffects {
    constructor(
        private actions$: Actions,
        private homeBoxService: HomeBoxService,
    ) { }

    loadTriggers$ = createEffect(() => this.actions$.pipe(
        ofType(TriggerActions.loadTriggers),
        mergeMap(() => this.homeBoxService.apiSecHomeBoxTriggersGet$Json()
            .pipe(
                map(triggers => TriggerActions.triggersLoadedSuccess({ triggers })),
                catchError(() => EMPTY)
            ))
    ));
}