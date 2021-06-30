import { Component, OnInit, OnDestroy } from '@angular/core';
import { EventLogService } from 'src/api/services';
import { Subscription } from 'rxjs';
import { DeviceDto, DeviceTypeDto } from 'src/api/models';


@Component({
  selector: 'app-home-box',
  templateUrl: './home-box.component.html',
})
export class HomeBoxComponent implements OnDestroy {

  private subscriptions: Subscription[] = [];

  isDeviceDetailsDialogVisible: boolean = false;
  isTriggerAddDialogVisible: boolean = false;
  selectedDevice: DeviceDto = { };
 
  showDeviceDetailsDialog(selectedDevice: DeviceDto){
    this.selectedDevice = selectedDevice;
    this.isDeviceDetailsDialogVisible = true;
  }

  hideDeviceDetailsDialog(){
    this.isDeviceDetailsDialogVisible = false;
  }

  showTriggerAddDialog(){
    this.isTriggerAddDialogVisible = true;
  }

  hideTriggerAddDialog(){
    this.isTriggerAddDialogVisible = false;
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}