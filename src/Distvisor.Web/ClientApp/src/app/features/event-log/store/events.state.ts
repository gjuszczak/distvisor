import { Injectable } from "@angular/core";
import { Action, Selector, State, StateContext } from "@ngxs/store";
import { of } from "rxjs";
import { catchError, map } from 'rxjs/operators';
import { EventDto } from "src/app/api/models";

import { EventLogService } from "src/app/api/services";
import { PaginatedList, isGuid } from "src/app/shared";

import { LoadEvents, LoadEventsFail, LoadEventsSuccess } from "./events.actions";

export interface EventsStateModel extends PaginatedList<EventDto> {
    aggregateId?: string,
};

export const eventsStateDefaults: EventsStateModel = {
    items: [],
    first: 0,
    rows: 10,
    rowsPerPageOptions: [10, 25, 50, 100],
    totalRecords: 0,
    loading: false,
    error: '',
};

@State<EventsStateModel>({
    name: 'events',
    defaults: eventsStateDefaults,
})
@Injectable()
export class EventsState {
    constructor(private readonly eventLogService: EventLogService) { }

    @Selector()
    static getEvents(state: EventsStateModel) {
        return state;
    }

    @Action(LoadEvents)
    loadEvents({ dispatch, getState, patchState }: StateContext<EventsStateModel>, action: LoadEvents) {
        const state = getState();
        const aggregateId = isGuid(action.aggregateId) 
            ? action.aggregateId
            : undefined;

        const first = (
            action.first
            && !isNaN(Number(action.first)))
            ? Number(action.first)
            : state.first;

        const rows = (
            action.rows
            && !isNaN(Number(action.rows))
            && state.rowsPerPageOptions.indexOf(Number(action.rows)) >= 0)
            ? Number(action.rows)
            : state.rows;

        patchState({
            loading: true,
            error: '',
        });

        return this.eventLogService.apiSEventLogGet$Json({ aggregateId, first, rows }).pipe(
            map(eventsList => dispatch(new LoadEventsSuccess(eventsList))),
            catchError(error => {
                dispatch(new LoadEventsFail(error));
                return of(new LoadEventsFail(error));
            })
        );
    }

    @Action(LoadEventsSuccess)
    loadEventsSuccess({ patchState }: StateContext<EventsStateModel>, { eventsList }: LoadEventsSuccess) {
        patchState({
            items: eventsList?.items ?? undefined,
            first: eventsList?.first ?? undefined,
            rows: eventsList?.rows ?? undefined,
            totalRecords: eventsList?.totalRecords ?? undefined,
            aggregateId: eventsList?.aggregateId ?? undefined,
            loading: false,
            error: ''
        });
    }

    @Action(LoadEventsFail)
    loadEventsFail({ patchState }: StateContext<EventsStateModel>, { error }: LoadEventsFail) {
        patchState({
            loading: false,
            error: error.message
        });
    }
}