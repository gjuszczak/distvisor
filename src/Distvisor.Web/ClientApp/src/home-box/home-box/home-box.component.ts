import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { HomeBoxDeviceDto } from 'src/api/models';
import { HomeBoxStore } from '../home-box.store';
import { HomeBoxState } from '../state/home-box.state';
import * as TriggerActions from '../state/triggers.actions';


@Component({
  selector: 'app-home-box',
  templateUrl: './home-box.component.html',
})
export class HomeBoxComponent implements OnInit, OnDestroy {
  private subscriptions: Subscription[] = [];
  
  readonly isTriggerAddDialogOpened$ = this.store.isTriggerAddDialogOpened$;

  isDeviceDetailsDialogVisible: boolean = false;
  isTriggerAddDialogVisible: boolean = false;
  selectedDevice: HomeBoxDeviceDto = {};
  devices: HomeBoxDeviceDto[] = [];

  constructor(private readonly store: HomeBoxStore, private readonly store2: Store<HomeBoxState>) {}

  ngOnInit(): void {
    this.store2.dispatch(TriggerActions.loadTriggers());
    //this.store.reloadDevices();
    //this.store.reloadTriggers();
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