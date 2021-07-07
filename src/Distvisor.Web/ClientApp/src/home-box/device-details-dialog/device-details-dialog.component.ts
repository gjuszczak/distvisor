import { Component, EventEmitter, Input, OnChanges, OnDestroy, Output, SimpleChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { HomeBoxDeviceDto, UpdateHomeBoxDeviceDto } from 'src/api/models';
import { HomeBoxService } from 'src/api/services';
import { UtilsService } from 'src/utils/utils.service';

@Component({
  selector: 'app-device-details-dialog',
  templateUrl: './device-details-dialog.component.html'
})
export class DeviceDetailsDialogComponent implements OnChanges, OnDestroy {
  @Input() isVisible: boolean = false;
  @Input() device: HomeBoxDeviceDto = {};
  @Output() onHide: EventEmitter<any> = new EventEmitter();

  private subscriptions: Subscription[] = [];

  header: string = '';
  location: string = '';
  params: string = '';

  constructor(private homeBoxService: HomeBoxService, private utils: UtilsService) {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['device']) {
      this.header = this.device.header ?? '';
      this.location = this.device.location ?? '';
      this.params = JSON.stringify(this.device.params ?? {}, null, 2);
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  onSaveClicked() {
    let objParams = JSON.parse(this.params);
    let diffParams = this.utils.diff(this.device.params, objParams);

    this.homeBoxService.apiSecHomeBoxDevicesIdentifierUpdateDetailsPost({
      identifier: this.device.id || '',
      body: {
        header: this.header,
        location: this.location,
        type: this.device.type,
        params: diffParams
      }
    }).subscribe(_ => {
      this.device.header = this.header;
      this.device.location = this.location;
      this.device.params = objParams;
    });

    this.onHide.emit();
  }

  onCancelClicked() {
    this.onHide.emit();
  }
}