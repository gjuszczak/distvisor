import { Component, EventEmitter, Input, OnChanges, OnDestroy, Output, SimpleChanges } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { Observable, Subscription } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { HomeBoxService } from 'src/api/services';
import { UtilsService } from 'src/app-common';
import { updateDevice } from '../state/devices.actions';
import { selectDeviceDetailsVmById } from '../state/devices.selectors';
import { closeDeviceDetailsDialog } from '../state/dialogs.actions';
import { selectDeviceDetailsDialogParam } from '../state/dialogs.selectors';
import { DeviceDetailsVm, HomeBoxState } from '../state/home-box.state';

@Component({
  selector: 'app-device-details-dialog',
  templateUrl: './device-details-dialog.component.html'
})
export class DeviceDetailsDialogComponent {

  private originalParams: string = '{}';
  readonly deviceDetailsVm$: Observable<DeviceDetailsVm | undefined> =
    this.store.select(selectDeviceDetailsDialogParam).pipe(
      switchMap(param => this.store.select(selectDeviceDetailsVmById(param.deviceId))),
      tap(x => this.originalParams = x?.params ?? '{}'),
    );
  isVisible: boolean = true;

  constructor(
    private homeBoxService: HomeBoxService,
    private utils: UtilsService,
    private store: Store<HomeBoxState>) {
  }

  onSave(deviceDetailsVm: DeviceDetailsVm | undefined) {
    if (!deviceDetailsVm) {
      //TODO: show error
      return;
    }

    let objOriginalParams = JSON.parse(this.originalParams);
    let objModifiedParams = JSON.parse(deviceDetailsVm.params);
    let diffParams = this.utils.diff(objOriginalParams, objModifiedParams);

    this.store.dispatch(updateDevice({
      device: {
        id: deviceDetailsVm.id,
        header: deviceDetailsVm.header,
        location: deviceDetailsVm.location,
        type: deviceDetailsVm.type,
        params: diffParams
      }
    }));
  }

  onCancel() {
    this.isVisible = false;
  }

  onHide() {
    this.store.dispatch(closeDeviceDetailsDialog());
  }
}