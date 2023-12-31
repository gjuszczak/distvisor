import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { EMPTY } from "rxjs";
import { catchError, map, mergeMap } from 'rxjs/operators';

import { DeviceDto } from 'src/app/api/models';
import { HomeBoxService } from 'src/app/api/services';
import { PaginatedList } from 'src/app/shared';

import { LoadDevices, LoadDevicesFail, LoadDevicesSuccess, SyncDevicesWithGateway } from './devices.actions';
import { patch } from '@ngxs/store/operators';

export interface DevicesList extends PaginatedList<DeviceDto> {
};

export interface DevicesStateModel {
    list: DevicesList
}

export const devicesStateDefaults: DevicesStateModel = {
    list: {
        items: [],
        first: 0,
        rows: 12,
        rowsPerPageOptions: [3, 6, 9, 12, 24],
        totalRecords: 0,
        loading: false,
        error: '',
    }
};

@State<DevicesStateModel>({
    name: 'devices',
    defaults: devicesStateDefaults,
})
@Injectable()
export class DevicesState {
    constructor(private readonly homeBoxService: HomeBoxService) { }

    @Selector()
    static getDevices(state: DevicesStateModel) {
        return state.list;
    }

    @Action(LoadDevices)
    loadDevices(ctx: StateContext<DevicesStateModel>, action: LoadDevices) {
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

        return this.homeBoxService.apiHomeBoxDevicesGet$Json({ first, rows }).pipe(
            mergeMap(devices => ctx.dispatch(new LoadDevicesSuccess(devices))),
            catchError(e => ctx.dispatch(new LoadDevicesFail(e.error.title)))
        );
    }

    @Action(LoadDevicesSuccess)
    loadDevicesSuccess(ctx: StateContext<DevicesStateModel>, { devices }: LoadDevicesSuccess) {
        ctx.setState(patch<DevicesStateModel>({
            list: patch<DevicesList>({
                items: devices?.items ?? [],
                first: devices?.first ?? undefined,
                rows: devices?.rows ?? undefined,
                totalRecords: devices?.totalRecords ?? undefined,
                loading: false,
                error: ''
            })
        }));
    }

    @Action(LoadDevicesFail)
    loadRedirectionsFail(ctx: StateContext<DevicesStateModel>, { error }: LoadDevicesFail) {
        ctx.setState(this.failListLoading(error));
    }

    @Action(SyncDevicesWithGateway)
    syncDevicesWithGateway(ctx: StateContext<DevicesStateModel>) {
        ctx.setState(this.startListLoading());

        return this.homeBoxService.apiHomeBoxDevicesSyncWithGatewayPost().pipe(
            map(() => ctx.dispatch(new LoadDevices())),
            catchError(() => EMPTY)
        );
    }

    private startListLoading() {
        return patch<DevicesStateModel>({
            list: patch<DevicesList>({
                loading: true,
                error: '',
            })
        });
    }

    private failListLoading(error: string) {
        return patch<DevicesStateModel>({
            list: patch<DevicesList>({
                items: [],
                loading: false,
                error: error ? error : "Unexpected error",
            })
        });
    }
}