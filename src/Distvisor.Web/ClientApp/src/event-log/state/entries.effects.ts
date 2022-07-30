import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { EMPTY } from 'rxjs';
import { catchError, map, mergeMap, withLatestFrom } from 'rxjs/operators';
import * as EventLogActions from './entries.actions';
import { EventLogState } from './event-log.state';
import { selectEventLogEntries } from './entries.selectors';
import { EventLogService } from 'src/api/services';

@Injectable()
export class EntriesEffects {
    constructor(
        private actions$: Actions,
        private eventLogService: EventLogService,
        private store: Store<EventLogState>
    ) { }

    loadEventLogEntries$ = createEffect(() => this.actions$.pipe(
        ofType(EventLogActions.loadEventLogEntries),
        withLatestFrom(this.store.select(selectEventLogEntries)),
        mergeMap(([action, eventLogEntries]) =>
            this.eventLogService.apiSEventLogGet$Json({
                firstOffset: action.firstOffset ?? eventLogEntries.firstOffset,
                pageSize: action.pageSize ?? eventLogEntries.pageSize
            }).pipe(
                map(eventLogEntries => EventLogActions.eventLogEntriesLoadedSuccess({ eventLogEntries })),
                catchError(() => EMPTY)
            ))
    ));
}