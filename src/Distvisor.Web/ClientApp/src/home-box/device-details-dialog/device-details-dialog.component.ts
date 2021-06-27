import { Component, EventEmitter, Input, OnChanges, OnDestroy, Output, SimpleChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { DeviceDto, DeviceTypeDto } from 'src/api/models';
import { HomeBoxService } from 'src/api/services';

@Component({
  selector: 'app-device-details-dialog',
  templateUrl: './device-details-dialog.component.html'
})
export class DeviceDetailsDialogComponent implements OnChanges, OnDestroy {
  @Input() isVisible: boolean = false;
  @Input() device: DeviceDto = {};
  @Output() onHide: EventEmitter<any> = new EventEmitter();

  private subscriptions: Subscription[] = [];

  name: string = '';
  location: string = '';
  params: string = '';

  constructor(private homeBoxService: HomeBoxService) {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['device']) {
      this.name = this.device.name ?? '';
      this.location = this.device.location ?? '';
      this.params = JSON.stringify(this.device.params ?? {}, null, 2);
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  onSaveClicked() {
    this.device.name = this.name;
    this.device.location = this.location;
    this.onHide.emit();
  }

  onCancelClicked() {
    this.onHide.emit();
  }
}