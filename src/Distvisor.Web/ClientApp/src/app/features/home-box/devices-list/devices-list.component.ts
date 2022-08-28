import { Component, OnDestroy, EventEmitter, Output } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { DeviceDto } from 'src/app/api/models';
import { HomeBoxStoreActions, HomeBoxStoreSelectors, RootStoreState } from 'src/app/root-store';

@Component({
  selector: 'app-devices-list',
  templateUrl: './devices-list.component.html',
})
export class DevicesListComponent implements OnDestroy {

  @Output() onDeviceDetailsOpen: EventEmitter<DeviceDto> = new EventEmitter();
  @Output() onDeviceListLoaded: EventEmitter<DeviceDto[]> = new EventEmitter();
  private subscriptions: Subscription[] = [];
  devices: DeviceDto[] = [];
  readonly devicesVm$ = this.store.pipe(select(HomeBoxStoreSelectors.selectDevicesVm));

  constructor(private readonly store: Store<RootStoreState.State>) { }

  onRefreshClicked() {
    this.store.dispatch(HomeBoxStoreActions.loadDevices());
  }

  onDeviceToggleClicked(device: DeviceDto) {
    // if (device.params?.switch === "on") {
    //   this.subscriptions.push(
    //     this.homeBoxService.apiSecHomeBoxDevicesIdentifierTurnOffPost({
    //       identifier: device.id ?? ''
    //     }).subscribe(_ => device.params.switch = "off")
    //   );
    // }

    // if (device.params?.switch === "off") {
    //   this.subscriptions.push(
    //     this.homeBoxService.apiSecHomeBoxDevicesIdentifierTurnOnPost({
    //       identifier: device.id ?? ''
    //     }).subscribe(_ => device.params.switch = "on")
    //   );
    // }
  }

  onDeviceShowDetailsClicked(deviceId: string) {
    this.store.dispatch(HomeBoxStoreActions.openDeviceDetailsDialog({ deviceId }));
    // this.onDeviceDetailsOpen.emit(device);
  }

  deviceListLoaded(devices: DeviceDto[]) {
    // this.devices = devices;
    // this.onDeviceListLoaded.emit(devices);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}