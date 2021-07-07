import { Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { HomeBoxDeviceDto } from 'src/api/models';


@Component({
  selector: 'app-home-box',
  templateUrl: './home-box.component.html',
})
export class HomeBoxComponent implements OnDestroy {

  private subscriptions: Subscription[] = [];

  isDeviceDetailsDialogVisible: boolean = false;
  isTriggerAddDialogVisible: boolean = false;
  selectedDevice: HomeBoxDeviceDto = { };
  devices: HomeBoxDeviceDto[] = [];
 
  showDeviceDetailsDialog(selectedDevice: HomeBoxDeviceDto){
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

  deviceListLoaded(devices: HomeBoxDeviceDto[]){
    this.devices = devices;
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}