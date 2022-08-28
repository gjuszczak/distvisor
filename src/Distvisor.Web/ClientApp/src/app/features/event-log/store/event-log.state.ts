import { Injectable } from "@angular/core";
import { Action, Selector, State, StateContext } from "@ngxs/store";
import { EMPTY } from "rxjs";
import { catchError, map } from 'rxjs/operators';

import { EventsLogEntryDto } from "src/app/api/models/events-log-entry-dto";
import { EventLogService } from "src/app/api/services";
import { PaginatedList } from "src/app/shared";

import { LoadEventLog, LoadEventLogSuccess } from "./event-log.actions";

export interface EventLogStateModel extends PaginatedList<EventsLogEntryDto> {
};

export const eventLogStateDefaults: EventLogStateModel = {
    items: [],
    firstOffset: 0,
    pageSize: 10,
    pageSizeOptions: [10, 25, 50, 100],
    totalCount: 0,
    loading: false
};

@State<EventLogStateModel>({
    name: 'eventLog',
    defaults: eventLogStateDefaults,
})
@Injectable()
export class EventLogState {
    constructor(private readonly eventLogService: EventLogService) { }

    @Selector()
    static getEventLog(state: EventLogStateModel) {
        return state;
    }

    @Action(LoadEventLog)
    loadEventLog({ dispatch, patchState }: StateContext<EventLogStateModel>, action: LoadEventLog) {
        patchState({
            loading: true,
        });

        return this.eventLogService.apiSEventLogGet$Json({
            firstOffset: action.firstOffset ?? eventLogStateDefaults.firstOffset,
            pageSize: action.pageSize ?? eventLogStateDefaults.pageSize
        }).pipe(
            map(eventLogList => dispatch(new LoadEventLogSuccess(eventLogList))),
            catchError(() => EMPTY)
        );
    }

    @Action(LoadEventLogSuccess)
    loadEventLogSuccess({ patchState }: StateContext<EventLogStateModel>, { eventLogList }: LoadEventLogSuccess) {
        patchState({
            items: eventLogList?.items ?? eventLogStateDefaults.items,
            firstOffset: eventLogList?.firstOffset ?? eventLogStateDefaults.firstOffset,
            pageSize: eventLogList?.pageSize ?? eventLogStateDefaults.pageSize,
            totalCount: eventLogList?.totalCount ?? eventLogStateDefaults.totalCount,
            loading: false
        });
    }
}