import { Component, OnInit, OnDestroy, EventEmitter, Output } from '@angular/core';
import { Subscription } from 'rxjs';
import { HomeBoxDeviceDto } from 'src/api/models';
import { HomeBoxService } from 'src/api/services';


@Component({
  selector: 'app-devices-list',
  templateUrl: './devices-list.component.html',
})
export class DevicesListComponent implements OnInit, OnDestroy {

  @Output() onDeviceDetailsOpen: EventEmitter<HomeBoxDeviceDto> = new EventEmitter();
  @Output() onDeviceListLoaded: EventEmitter<HomeBoxDeviceDto[]> = new EventEmitter();
  private subscriptions: Subscription[] = [];
  devices: HomeBoxDeviceDto[] = [];

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

  onDeviceToggleClicked(device: HomeBoxDeviceDto) {
    if (device.params?.switch === "on") {
      this.subscriptions.push(
        this.homeBoxService.apiSecHomeBoxDevicesIdentifierTurnOffPost({
          identifier: device.id ?? ''
        }).subscribe(_ => device.params.switch = "off")
      );
    }

    if (device.params?.switch === "off") {
      this.subscriptions.push(
        this.homeBoxService.apiSecHomeBoxDevicesIdentifierTurnOnPost({
          identifier: device.id ?? ''
        }).subscribe(_ => device.params.switch = "on")
      );
    }
  }

  onDeviceShowDetailsClicked(device: HomeBoxDeviceDto) {
    this.onDeviceDetailsOpen.emit(device);
  }

  deviceListLoaded(devices: HomeBoxDeviceDto[]) {
    this.devices = devices;
    this.onDeviceListLoaded.emit(devices);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}