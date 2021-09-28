import { Component, OnInit, OnDestroy, EventEmitter, Output } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import * as DevicesActions from '../state/devices.actions';
import { HomeBoxDeviceDto } from 'src/api/models';
import { selectDevicesVm } from '../state/devices.selectors';
import { HomeBoxState } from '../state/home-box.state';


@Component({
  selector: 'app-devices-list',
  templateUrl: './devices-list.component.html',
})
export class DevicesListComponent implements OnDestroy {

  @Output() onDeviceDetailsOpen: EventEmitter<HomeBoxDeviceDto> = new EventEmitter();
  @Output() onDeviceListLoaded: EventEmitter<HomeBoxDeviceDto[]> = new EventEmitter();
  private subscriptions: Subscription[] = [];
  devices: HomeBoxDeviceDto[] = [];
  readonly devicesVm$ = this.store.pipe(select(selectDevicesVm));


  constructor(
    private store: Store<HomeBoxState>) { }

  onRefreshClicked() {
    this.store.dispatch(DevicesActions.loadDevices());
  }

  onDeviceToggleClicked(device: HomeBoxDeviceDto) {
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

  onDeviceShowDetailsClicked(device: HomeBoxDeviceDto) {
    // this.onDeviceDetailsOpen.emit(device);
  }

  deviceListLoaded(devices: HomeBoxDeviceDto[]) {
    // this.devices = devices;
    // this.onDeviceListLoaded.emit(devices);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}