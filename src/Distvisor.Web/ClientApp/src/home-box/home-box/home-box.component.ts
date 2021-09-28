import { Component, OnDestroy, OnInit } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { HomeBoxDeviceDto } from 'src/api/models';
import { selectIsTriggerAddDialogOpened } from '../state/dialogs.selectors';
import { HomeBoxState } from '../state/home-box.state';
import * as TriggerActions from '../state/triggers.actions';
import * as DevicesActions from '../state/devices.actions';


@Component({
  selector: 'app-home-box',
  templateUrl: './home-box.component.html',
})
export class HomeBoxComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  
  readonly isTriggerAddDialogOpened$ = this.store.pipe(select(selectIsTriggerAddDialogOpened));

  isDeviceDetailsDialogVisible: boolean = false;
  isTriggerAddDialogVisible: boolean = false;
  selectedDevice: HomeBoxDeviceDto = {};
  devices: HomeBoxDeviceDto[] = [];

  constructor(private readonly store: Store<HomeBoxState>) {}

  ngOnInit(): void {
    this.store.dispatch(TriggerActions.loadTriggers());
    this.store.dispatch(DevicesActions.loadDevices());
  }

  showDeviceDetailsDialog(selectedDevice: HomeBoxDeviceDto) {
    this.selectedDevice = selectedDevice;
    this.isDeviceDetailsDialogVisible = true;
  }

  hideDeviceDetailsDialog() {
    this.isDeviceDetailsDialogVisible = false;
  }

  showTriggerAddDialog() {
    this.isTriggerAddDialogVisible = true;
  }

  hideTriggerAddDialog() {
    this.isTriggerAddDialogVisible = false;
  }

  deviceListLoaded(devices: HomeBoxDeviceDto[]) {
    this.devices = devices;
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}