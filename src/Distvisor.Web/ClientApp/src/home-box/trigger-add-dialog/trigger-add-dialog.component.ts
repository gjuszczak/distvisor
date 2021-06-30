import { Component, EventEmitter, Input, OnChanges, OnDestroy, Output, SimpleChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { HomeBoxTriggerDto, HomeBoxTriggerSource, HomeBoxTriggerSourceType } from 'src/api/models';
import { HomeBoxService } from 'src/api/services';

@Component({
  selector: 'app-trigger-add-dialog',
  templateUrl: './trigger-add-dialog.component.html'
})
export class TriggerAddDialogComponent implements OnChanges, OnDestroy {
  @Input() isVisible: boolean = false;
  @Output() onHide: EventEmitter<any> = new EventEmitter();

  private subscriptions: Subscription[] = [];
  private trigger: HomeBoxTriggerDto;

  sourceTypes: any = [
    { name: "RF 433 Receiver", value: HomeBoxTriggerSourceType.Rf433Receiver }
  ]
  selectedSourceType: HomeBoxTriggerSourceType;
  selectedSourceMatchParam: string = "";
  triggerSources: HomeBoxTriggerSource[] = []

  constructor(private homeBoxService: HomeBoxService) {
    this.trigger = this.initTrigger();
    this.selectedSourceType = this.sourceTypes[0].value;
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['isVisible']) {
      this.trigger = this.initTrigger();
      this.selectedSourceType = this.sourceTypes[0].value;
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  onAddTriggerSource() {
    this.triggerSources.push({ type: this.selectedSourceType, matchParam: this.selectedSourceMatchParam });
    this.selectedSourceMatchParam = "";
  }

  onDeleteTriggerSource(triggerSource: HomeBoxTriggerSource) {
    let index = this.triggerSources.indexOf(triggerSource);
    console.log(index);
    this.triggerSources.splice(index, 1);
  }

  onSaveClicked() {
    this.onHide.emit();
  }

  onCancelClicked() {
    this.onHide.emit();
  }

  initTrigger = () => <HomeBoxTriggerDto>{
  }
}