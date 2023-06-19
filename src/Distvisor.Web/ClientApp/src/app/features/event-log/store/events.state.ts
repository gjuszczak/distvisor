import { Injectable } from "@angular/core";
import { Action, Selector, State, StateContext } from "@ngxs/store";
import { EMPTY } from "rxjs";
import { catchError, mergeMap } from 'rxjs/operators';
import { EventDto } from "src/app/api/models";

import { EventLogService } from "src/app/api/services";
import { PaginatedList, isGuid } from "src/app/shared";

import { LoadEvents, LoadEventsFail, LoadEventsSuccess, ReplayEvents, ShowEventsModal, HideEventsModal, FailEventsModal } from "./events.actions";
import { patch } from "@ngxs/store/operators";

export interface EventsList extends PaginatedList<EventDto> {
    aggregateId?: string,
};

export enum EventsModals {
    None,
    ReplayEvents
};

export interface EventsModal {
    visible: EventsModals;
    loading: boolean;
    error: string;
};

export interface EventsStateModel {
    list: EventsList,
    modal: EventsModal;
};

export const eventsStateDefaults: EventsStateModel = {
    list: {
        items: [],
        first: 0,
        rows: 10,
        rowsPerPageOptions: [10, 25, 50, 100],
        totalRecords: 0,
        loading: false,
        error: ''
    },
    modal: {
        visible: EventsModals.None,
        loading: false,
        error: ''
    }
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
        return state.list;
    }

    @Selector()
    static getModal(state: EventsStateModel) {
        return state.modal;
    }

    @Action(LoadEvents)
    loadEvents(ctx: StateContext<EventsStateModel>, action: LoadEvents) {
        const state = ctx.getState();
        const aggregateId = isGuid(action.aggregateId)
            ? action.aggregateId
            : undefined;

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

        return this.eventLogService.apiEventLogEventsGet$Json({ aggregateId, first, rows }).pipe(
            mergeMap(eventsList => ctx.dispatch(new LoadEventsSuccess(eventsList))),
            catchError(e => ctx.dispatch(new LoadEventsFail(e.error.title)))
        );
    }

    @Action(LoadEventsSuccess)
    loadEventsSuccess(ctx: StateContext<EventsStateModel>, { eventsList }: LoadEventsSuccess) {
        ctx.setState(patch<EventsStateModel>({
            list: patch<EventsList>({
                items: eventsList?.items ?? undefined,
                first: eventsList?.first ?? undefined,
                rows: eventsList?.rows ?? undefined,
                totalRecords: eventsList?.totalRecords ?? undefined,
                aggregateId: eventsList?.aggregateId ?? undefined,
                loading: false,
                error: ''
            })
        }));
    }

    @Action(LoadEventsFail)
    loadEventsFail(ctx: StateContext<EventsStateModel>, { error }: LoadEventsFail) {
        ctx.setState(this.failListLoading(error));
    }

    @Action(ShowEventsModal)
    showBackupModal(ctx: StateContext<EventsStateModel>, { modal }: ShowEventsModal) {
        ctx.setState(this.setVisibleModal(modal));
    }

    @Action(HideEventsModal)
    hideBackupModal(ctx: StateContext<EventsStateModel>, { reloadEvents }: HideEventsModal) {
        ctx.setState(this.setVisibleModal(EventsModals.None));

        return reloadEvents
            ? ctx.dispatch(new LoadEvents())
            : EMPTY;
    }

    @Action(FailEventsModal)
    failBackupModal(ctx: StateContext<EventsStateModel>, { error }: FailEventsModal) {
        ctx.setState(this.endModalLoading(error));
    }

    @Action(ReplayEvents)
    replayEvents(ctx: StateContext<EventsStateModel>) {
        ctx.setState(this.startModalLoading());

        return this.eventLogService.apiEventLogReplayEventsPost$Response({ body: {} }).pipe(
            mergeMap(() => ctx.dispatch(new HideEventsModal(true))),
            catchError((e: any) => {debugger; return ctx.dispatch(new FailEventsModal(e.error.title));})
        );
    }

    private startListLoading() {
        return patch<EventsStateModel>({
            list: patch<EventsList>({
                loading: true,
                error: '',
            })
        });
    }

    private failListLoading(error: string) {
        return patch<EventsStateModel>({
            list: patch<EventsList>({
                items: [],
                loading: false,
                error: error ? error : "Unexpected error",
            })
        });
    }

    private setVisibleModal(modal: EventsModals) {
        return patch<EventsStateModel>({
            modal: patch<EventsModal>({
                visible: modal,
                loading: false,
                error: '',
            })
        });
    }

    private startModalLoading() {
        return patch<EventsStateModel>({
            modal: patch<EventsModal>({
                loading: true,
                error: '',
            })
        });
    }

    private endModalLoading(error: string) {
        return patch<EventsStateModel>({
            modal: patch<EventsModal>({
                loading: false,
                error: error,
            })
        });
    }
}