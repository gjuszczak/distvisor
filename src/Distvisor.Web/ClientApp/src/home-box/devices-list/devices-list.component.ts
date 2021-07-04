import { Component, OnInit, OnDestroy, EventEmitter, Output } from '@angular/core';
import { Subscription } from 'rxjs';
import { DeviceDto } from 'src/api/models/device-dto';
import { HomeBoxService } from 'src/api/services';


@Component({
  selector: 'app-devices-list',
  templateUrl: './devices-list.component.html',
})
export class DevicesListComponent implements OnInit, OnDestroy {

  @Output() onDeviceDetailsOpen: EventEmitter<DeviceDto> = new EventEmitter();
  @Output() onDeviceListLoaded: EventEmitter<DeviceDto[]> = new EventEmitter();
  private subscriptions: Subscription[] = [];
  devices: DeviceDto[] = [];

  constructor(private homeBoxService: HomeBoxService) {
  }

  ngOnInit() {
    this.reloadList();
  }

  reloadList() {
    this.subscriptions.push(
      this.homeBoxService.apiSecHomeBoxDevicesGet$Json()
        .subscribe(devices => {
          this.deviceListLoaded(devices);
        }));
  }

  onDeviceToggleClicked(device: DeviceDto) {
    if (device.params?.switch === "on") {
      this.subscriptions.push(
        this.homeBoxService.apiSecHomeBoxDevicesIdentifierTurnOffPost({
          identifier: device.identifier ?? ''
        }).subscribe(_ => device.params.switch = "off")
      );
    }

    if (device.params?.switch === "off") {
      this.subscriptions.push(
        this.homeBoxService.apiSecHomeBoxDevicesIdentifierTurnOnPost({
          identifier: device.identifier ?? ''
        }).subscribe(_ => device.params.switch = "on")
      );
    }
  }

  onDeviceShowDetailsClicked(device: DeviceDto) {
    this.onDeviceDetailsOpen.emit(device);
  }

  deviceListLoaded(devices: DeviceDto[]) {
    this.devices = devices;
    this.onDeviceListLoaded.emit(devices);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}