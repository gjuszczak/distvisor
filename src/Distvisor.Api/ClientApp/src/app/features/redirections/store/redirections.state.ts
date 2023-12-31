import { Injectable } from "@angular/core";
import { Action, Selector, State, StateContext } from "@ngxs/store";
import { patch } from '@ngxs/store/operators';
import { EMPTY, Observable } from "rxjs";
import { catchError, mergeMap } from 'rxjs/operators';

import { RedirectionsService } from "src/app/api/services";
import { RedirectionDto } from "src/app/api/models";

import { PaginatedList } from "src/app/shared";
import {
    CreateRedirection,
    DeleteRedirection,
    EditRedirection,
    FailRedirectionModal,
    HideRedirectionModal,
    LoadRedirections,
    LoadRedirectionsFail,
    LoadRedirectionsSuccess,
    ShowRedirectionModal
} from "./redirections.actions";

export interface Redirection {
    id: string;
    name: string;
    url: string;
}

export interface RedirectionsList extends PaginatedList<Redirection> {
};

export enum RedirectionModalKind {
    None,
    Create,
    Edit,
    ConfirmDelete
};

export interface RedirectionModal {
    kind: RedirectionModalKind;
    loading: boolean;
    value: Redirection,
    error: string;
};

export interface RedirectionsStateModel {
    list: RedirectionsList,
    modal: RedirectionModal;
};

export const emptyRedirection: Redirection = {
    id: '',
    name: '',
    url: '',
}

export const redirectionsStateDefaults: RedirectionsStateModel = {
    list: {
        items: [],
        first: 0,
        rows: 10,
        rowsPerPageOptions: [10, 25, 50, 100],
        totalRecords: 0,
        loading: false,
        error: '',
    },
    modal: {
        kind: RedirectionModalKind.None,
        error: '',
        loading: false,
        value: emptyRedirection
    }
};

@State<RedirectionsStateModel>({
    name: 'redirections',
    defaults: redirectionsStateDefaults,
})
@Injectable()
export class RedirectionsState {
    constructor(private readonly redirectionsService: RedirectionsService) { }

    @Selector()
    static getRedirections(state: RedirectionsStateModel) {
        return state.list;
    }

    @Selector()
    static getModal(state: RedirectionsStateModel) {
        return state.modal;
    }

    @Action(LoadRedirections)
    loadRedirections(ctx: StateContext<RedirectionsStateModel>, action: LoadRedirections) {
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

        return this.redirectionsService.apiRedirectionsGet$Json({ first, rows }).pipe(
            mergeMap(redirections => ctx.dispatch(new LoadRedirectionsSuccess(redirections))),
            catchError(e => ctx.dispatch(new LoadRedirectionsFail(e.error.title)))
        );
    }

    @Action(LoadRedirectionsSuccess)
    loadRedirectionsSuccess(ctx: StateContext<RedirectionsStateModel>, { redirections }: LoadRedirectionsSuccess) {
        ctx.setState(patch<RedirectionsStateModel>({
            list: patch<RedirectionsList>({
                items: this.dtosToRedirections(redirections?.items ?? []),
                first: redirections?.first ?? undefined,
                rows: redirections?.rows ?? undefined,
                totalRecords: redirections?.totalRecords ?? undefined,
                loading: false,
                error: ''
            })
        }));
    }

    @Action(LoadRedirectionsFail)
    loadRedirectionsFail(ctx: StateContext<RedirectionsStateModel>, { error }: LoadRedirectionsFail) {
        ctx.setState(this.failListLoading(error));
    }

    @Action(ShowRedirectionModal)
    showRedirectionModal(ctx: StateContext<RedirectionsStateModel>, { modal, value }: ShowRedirectionModal) {
        ctx.setState(this.setModal(modal, value));
    }

    @Action(HideRedirectionModal)
    hideRedirectionModal(ctx: StateContext<RedirectionsStateModel>, { reloadRedirections }: HideRedirectionModal) {
        ctx.setState(this.setModal(RedirectionModalKind.None, emptyRedirection));

        return reloadRedirections
            ? ctx.dispatch(new LoadRedirections())
            : EMPTY;
    }

    @Action(FailRedirectionModal)
    failRedirectionModal(ctx: StateContext<RedirectionsStateModel>, { error }: FailRedirectionModal) {
        ctx.setState(this.failModalLoading(error));
    }

    @Action(CreateRedirection)
    createRedirection(ctx: StateContext<RedirectionsStateModel>, { name, url }: CreateRedirection) {
        ctx.setState(this.startModalLoading());

        return this.redirectionsService.apiRedirectionsPost({ body: { name, url } }).pipe(
            this.handleModalFetchResponse(ctx)
        );
    }

    @Action(EditRedirection)
    editRedirection(ctx: StateContext<RedirectionsStateModel>, { redirectionId, name, url }: EditRedirection) {
        ctx.setState(this.startModalLoading());

        return this.redirectionsService.apiRedirectionsPatch({ body: { redirectionId, name, url } }).pipe(
            this.handleModalFetchResponse(ctx)
        );
    }

    @Action(DeleteRedirection)
    releteRedirection(ctx: StateContext<RedirectionsStateModel>, { redirectionId }: DeleteRedirection) {
        ctx.setState(this.startModalLoading());

        return this.redirectionsService.apiRedirectionsDelete({ body: { redirectionId } }).pipe(
            this.handleModalFetchResponse(ctx)
        );
    }

    private startListLoading() {
        return patch<RedirectionsStateModel>({
            list: patch<RedirectionsList>({
                loading: true,
                error: '',
            })
        });
    }

    private failListLoading(error: string) {
        return patch<RedirectionsStateModel>({
            list: patch<RedirectionsList>({
                items: [],
                loading: false,
                error: error ? error : "Unexpected error",
            })
        });
    }

    private setModal(kind: RedirectionModalKind, value: Redirection) {
        return patch<RedirectionsStateModel>({
            modal: {
                kind: kind,
                loading: false,
                value: value,
                error: '',
            }
        });
    }

    private startModalLoading() {
        return patch<RedirectionsStateModel>({
            modal: patch<RedirectionModal>({
                loading: true,
                error: '',
            })
        });
    }

    private failModalLoading(error: string) {
        return patch<RedirectionsStateModel>({
            modal: patch<RedirectionModal>({
                loading: false,
                error: error ? error : "Unexpected error",
            })
        });
    }

    private handleModalFetchResponse<T>(ctx: StateContext<RedirectionsStateModel>) {
        return (source: Observable<T>) =>
            source.pipe(
                mergeMap(() => ctx.dispatch(new HideRedirectionModal(true))),
                catchError((e: any) => ctx.dispatch(new FailRedirectionModal(e.error.title)))
            );
    };

    private dtosToRedirections(dtos: RedirectionDto[]) {
        const redirections: Redirection[] = dtos
            .filter(dto => dto.id && dto.name && dto.url)
            .map(dto => <Redirection>{
                id: dto.id,
                name: dto.name,
                url: dto.url
            });

        const invalidCount = dtos.length - redirections.length;
        if (invalidCount > 0) {
            console.error(`${invalidCount} invalid RedirectionDtos received.`);
        }

        return redirections;
    }
}
