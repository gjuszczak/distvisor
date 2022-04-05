import { Component, OnDestroy, EventEmitter, Output } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { DeviceDto } from 'src/api/models';
import { selectDevicesVm } from '../state/devices.selectors';
import { HomeBoxState } from '../state/home-box.state';
import { loadDevices } from '../state/devices.actions';
import { openDeviceDetailsDialog } from '../state/dialogs.actions';


@Component({
  selector: 'app-devices-list',
  templateUrl: './devices-list.component.html',
})
export class DevicesListComponent implements OnDestroy {

  @Output() onDeviceDetailsOpen: EventEmitter<DeviceDto> = new EventEmitter();
  @Output() onDeviceListLoaded: EventEmitter<DeviceDto[]> = new EventEmitter();
  private subscriptions: Subscription[] = [];
  devices: DeviceDto[] = [];
  readonly devicesVm$ = this.store.pipe(select(selectDevicesVm));


  constructor(
    private store: Store<HomeBoxState>) { }

  onRefreshClicked() {
    this.store.dispatch(loadDevices());
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
    this.store.dispatch(openDeviceDetailsDialog({ deviceId }));
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