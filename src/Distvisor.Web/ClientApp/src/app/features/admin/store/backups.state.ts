import { Injectable } from "@angular/core";
import { Action, Selector, State, StateContext } from "@ngxs/store";
import { patch } from '@ngxs/store/operators';
import { EMPTY, Observable } from "rxjs";
import { catchError, mergeMap } from 'rxjs/operators';

import { AdminService } from "src/app/api/services";
import { BackupFileDto } from "src/app/api/models";

import { PaginatedList } from "src/app/shared";
import {
    CreateBackup,
    DeleteBackup,
    FailBackupModal,
    HideBackupModal,
    LoadBackups,
    LoadBackupsFail,
    LoadBackupsSuccess,
    RenameBackup,
    RestoreBackup,
    ShowBackupModal
} from "./backups.actions";


export interface BackupsList extends PaginatedList<BackupFileDto> {
};

export enum BackupModals {
    None,
    Create,
    Rename,
    ConfirmDelete,
    ConfirmRestore
}

export interface BackupModal {
    visible: BackupModals;
    loading: boolean;
    defaultInputText: string,
    error: string;
};

export interface BackupsStateModel {
    list: BackupsList,
    modal: BackupModal;
};

export const backupsStateDefaults: BackupsStateModel = {
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
        visible: BackupModals.None,
        loading: false,
        defaultInputText: '',
        error: ''
    }
};

@State<BackupsStateModel>({
    name: 'backups',
    defaults: backupsStateDefaults,
})
@Injectable()
export class BackupsState {
    constructor(private readonly adminService: AdminService) { }

    @Selector()
    static getBackups(state: BackupsStateModel) {
        return state.list;
    }

    @Selector()
    static getModal(state: BackupsStateModel) {
        return state.modal;
    }

    @Action(LoadBackups)
    loadBackups(ctx: StateContext<BackupsStateModel>, action: LoadBackups) {
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

        return this.adminService.apiSAdminBackupsGet$Json({ first, rows }).pipe(
            mergeMap(backups => ctx.dispatch(new LoadBackupsSuccess(backups))),
            catchError(e => ctx.dispatch(new LoadBackupsFail(e.error)))
        );
    }

    @Action(LoadBackupsSuccess)
    loadBackupsSuccess(ctx: StateContext<BackupsStateModel>, { backups }: LoadBackupsSuccess) {
        ctx.setState(patch<BackupsStateModel>({
            list: patch<BackupsList>({
                items: backups?.items ?? undefined,
                first: backups?.first ?? undefined,
                rows: backups?.rows ?? undefined,
                totalRecords: backups?.totalRecords ?? undefined,
                loading: false,
                error: ''
            })
        }));
    }

    @Action(LoadBackupsFail)
    loadBackupsFail(ctx: StateContext<BackupsStateModel>, { error }: LoadBackupsFail) {
        ctx.setState(this.endListLoading(error));
    }

    @Action(HideBackupModal)
    hideBackupModal(ctx: StateContext<BackupsStateModel>, { reloadBackups }: HideBackupModal) {
        ctx.setState(this.setVisibleModal(BackupModals.None));

        return reloadBackups
            ? ctx.dispatch(new LoadBackups())
            : EMPTY;
    }

    @Action(ShowBackupModal)
    showBackupModal(ctx: StateContext<BackupsStateModel>, { modal, defaultInputText }: ShowBackupModal) {
        ctx.setState(this.setVisibleModal(modal, defaultInputText));
    }

    @Action(FailBackupModal)
    failBackupModal(ctx: StateContext<BackupsStateModel>, { error }: FailBackupModal) {
        ctx.setState(this.endModalLoading(error));
    }

    @Action(CreateBackup)
    createBackup(ctx: StateContext<BackupsStateModel>, { name }: CreateBackup) {
        ctx.setState(this.startModalLoading());

        return this.adminService.apiSAdminBackupsPost({ body: { name: name } }).pipe(
            this.handleModalFetchResponse(ctx)
        );
    }

    @Action(RenameBackup)
    renameBackup(ctx: StateContext<BackupsStateModel>, { oldName, newName }: RenameBackup) {
        ctx.setState(this.startModalLoading());

        return this.adminService.apiSAdminBackupsPatch({ body: { oldName: oldName, newName: newName } }).pipe(
            this.handleModalFetchResponse(ctx)
        );
    }

    @Action(DeleteBackup)
    deleteBackup(ctx: StateContext<BackupsStateModel>, { name }: DeleteBackup) {
        ctx.setState(this.startModalLoading());

        return this.adminService.apiSAdminBackupsDelete({ body: { name: name } }).pipe(
            this.handleModalFetchResponse(ctx)
        );
    }

    @Action(RestoreBackup)
    restoreBackup(ctx: StateContext<BackupsStateModel>, { name }: RestoreBackup) {
        ctx.setState(this.startModalLoading());

        return this.adminService.apiSAdminBackupsRestorePost({ body: { name: name } }).pipe(
            this.handleModalFetchResponse(ctx)
        );
    }

    private startListLoading() {
        return patch<BackupsStateModel>({
            list: patch<BackupsList>({
                loading: true,
                error: '',
            })
        });
    }

    private endListLoading(error: string = '') {
        return patch<BackupsStateModel>({
            list: patch<BackupsList>({
                loading: false,
                error: error,
            })
        });
    }

    private setVisibleModal(modal: BackupModals, defaultInputText: string = '') {
        return patch<BackupsStateModel>({
            modal: patch<BackupModal>({
                visible: modal,
                loading: false,
                defaultInputText: defaultInputText,
                error: '',
            })
        });
    }

    private startModalLoading() {
        return patch<BackupsStateModel>({
            modal: patch<BackupModal>({
                loading: true,
                error: '',
            })
        });
    }

    private endModalLoading(error: string = '') {
        return patch<BackupsStateModel>({
            modal: patch<BackupModal>({
                loading: false,
                error: error,
            })
        });
    }

    private handleModalFetchResponse<T>(ctx: StateContext<BackupsStateModel>) {
        return (source: Observable<T>) =>
            source.pipe(
                mergeMap(() => ctx.dispatch(new HideBackupModal(true))),
                catchError((e: any) => ctx.dispatch(new FailBackupModal(e.error)))
            );
    };
}
