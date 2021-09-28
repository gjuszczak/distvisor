import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { EMPTY } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import * as DeviceActions from './devices.actions';
import { HomeBoxService } from 'src/api/services';


@Injectable()
export class DevicesEffects {
    constructor(
        private actions$: Actions,
        private homeBoxService: HomeBoxService,
    ) { }

    loadDevices$ = createEffect(() => this.actions$.pipe(
        ofType(DeviceActions.loadDevices),
        mergeMap(() => 
            this.homeBoxService.apiSecHomeBoxDevicesGet$Json().pipe(
                map(devices => DeviceActions.devicesLoadedSuccess({ devices })),
                catchError(() => EMPTY)
            ))
    ));
}