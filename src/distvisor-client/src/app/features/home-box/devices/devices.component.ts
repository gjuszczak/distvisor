import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { MenuItem } from 'primeng/api';

import { DeviceDto } from 'src/app/api/models';
import { LoadDevices, SyncDevicesWithGateway } from '../store/devices.actions';
import { DevicesList, DevicesState } from '../store/devices.state';
import { TableLazyLoadEvent } from 'primeng/table';

@Component({
  selector: 'app-devices',
  templateUrl: './devices.component.html',
})
export class DevicesComponent implements OnInit {
  
  @Select(DevicesState.getDevices)
  public readonly devices$!: Observable<DevicesList>;

  public devicesMenuItems: MenuItem[] = [];
  private firstLazyLoad: boolean = true;
  
  constructor(
    private readonly store: Store,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
  ) { }

  ngOnInit(): void {
    this.devicesMenuItems = [
      {
        label: 'Sync with gateway',
        icon: 'pi pi-sync',
        command: () => {
          this.syncDevicesWithGateway();
        }
      }
    ];

    this.route.queryParams.subscribe(({ first, rows }) => {
      this.reloadDevices(first, rows);
    });
  }

  lazyLoadDevices({ first, rows }: TableLazyLoadEvent) {
    if (this.firstLazyLoad) {
      this.firstLazyLoad = false;
      return;
    }

    this.router.navigate([], { queryParams: { first, rows } });
  }

  reloadDevices(first?: number, rows?: number) {
    this.store.dispatch(new LoadDevices(first, rows));
  }

  syncDevicesWithGateway() {
    this.store.dispatch(new SyncDevicesWithGateway());
  }

  onDeviceToggleClicked(device: DeviceDto) {

  }

  onDeviceShowDetailsClicked(deviceId: string) {

  }
}
