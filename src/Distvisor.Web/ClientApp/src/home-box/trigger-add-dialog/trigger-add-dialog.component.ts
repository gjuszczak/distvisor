import { ChangeDetectorRef, Component, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { HomeBoxTriggerAction, HomeBoxTriggerDto, HomeBoxTriggerSource, HomeBoxTriggerSourceType, HomeBoxTriggerTarget } from 'src/api/models';
import { RfCodeService } from 'src/notifications/rfcode.service';
import { HomeBoxStore, NameValue } from '../home-box.store';

@Component({
  selector: 'app-trigger-add-dialog',
  templateUrl: './trigger-add-dialog.component.html'
})
export class TriggerAddDialogComponent implements OnDestroy {
  private subscriptions: Subscription[] = [];
  private syncMatchParamSubscription: Subscription | null = null;

  isVisible: boolean = true;
  sourceTypes: NameValue<HomeBoxTriggerSourceType>[] = [
    { name: "RF 433 Receiver", value: HomeBoxTriggerSourceType.Rf433Receiver }
  ]
  actionOnOffOptions: NameValue<boolean | null>[] = [
    { name: "Don't care", value: null },
    { name: "On", value: true },
    { name: "Off", value: false }
  ]
  selectedSourceType: NameValue<HomeBoxTriggerSourceType>;
  selectedSourceMatchParam: string = "";
  selectedTriggerAction: number = 0;
  targetOptions: NameValue<string>[] = [];
  selectedTarget: NameValue<string> | null = null;
  triggerSources: NameValue<HomeBoxTriggerSource>[] = [];
  triggerTargets: NameValue<HomeBoxTriggerTarget>[] = [];
  triggerActions: HomeBoxTriggerAction[] = [];
  isSyncMatchParamInProgress: boolean = false;

  constructor(
    private rfCodeService: RfCodeService,
    private cdref: ChangeDetectorRef,
    private store: HomeBoxStore) {
    this.selectedSourceType = this.sourceTypes[0];
    this.triggerActions.push(this.emptyAction());
    this.subscriptions.push(
      this.store.devicesShortVm$.subscribe(devices => {
        this.targetOptions = devices;
        this.selectedTarget = this.targetOptions.length ? this.targetOptions[0] : null;
      })
    )
  }

  ngOnDestroy() {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

  onAddTriggerSource() {
    if (!this.selectedSourceType) {
      return;
    }
    if (this.triggerSources.find(t => t.value.type === this.selectedSourceType?.value && t.value.matchParam === this.selectedSourceMatchParam)) {
      return;
    }
    this.triggerSources.push({
      name: this.selectedSourceType.name,
      value: {
        type: this.selectedSourceType.value,
        matchParam: this.selectedSourceMatchParam
      }
    });
    this.selectedSourceMatchParam = "";
  }

  onDeleteTriggerSource(triggerSource: NameValue<HomeBoxTriggerSource>) {
    let index = this.triggerSources.indexOf(triggerSource);
    if (index != -1) {
      this.triggerSources.splice(index, 1);
    }
  }

  onAddTriggerTarget() {
    if (!this.selectedTarget) {
      return;
    }
    if (this.triggerTargets.find(t => t.value.deviceIdentifier === this.selectedTarget?.value)) {
      return;
    }
    this.triggerTargets.push({
      name: this.selectedTarget.name,
      value: {
        deviceIdentifier: this.selectedTarget.value
      }
    });
  }

  onDeleteTriggerTarget(triggerTarget: NameValue<HomeBoxTriggerTarget>) {
    let index = this.triggerTargets.indexOf(triggerTarget);
    if (index != -1) {
      this.triggerTargets.splice(index, 1);
    }
  }

  onAddTriggerAction() {
    let action = this.emptyAction();
    this.triggerActions.splice(this.selectedTriggerAction + 1, 0, action);
    this.cdref.detectChanges();
    this.selectedTriggerAction = this.selectedTriggerAction + 1;
  }

  onDeleteTriggerAction() {
    if (this.triggerActions.length <= 1) {
      return;
    }
    this.triggerActions.splice(this.selectedTriggerAction, 1);
    if (this.selectedTriggerAction >= this.triggerActions.length) {
      this.selectedTriggerAction = this.selectedTriggerAction - 1;
    }
    this.cdref.detectChanges();
  }

  emptyAction(): HomeBoxTriggerAction {
    return {
      isDeviceOn: null,
      lastExecutedActionNumber: null,
      lastExecutedActionMinDelayMs: null,
      lastExecutedActionMaxDelayMs: null,
      payload: JSON.stringify({ switch: "on" }, null, 2)
    }
  }

  onSyncMatchParam() {
    if (this.isSyncMatchParamInProgress) {
      this.syncMatchParamSubscription?.unsubscribe();
      this.syncMatchParamSubscription = null;
      this.isSyncMatchParamInProgress = false;
    }
    else {
      this.selectedSourceMatchParam = '';
      this.isSyncMatchParamInProgress = true;
      this.syncMatchParamSubscription = this.rfCodeService.rfCodeSubject$.subscribe(code => {
        this.selectedSourceMatchParam = code;
        this.isSyncMatchParamInProgress = false;
        this.syncMatchParamSubscription?.unsubscribe();
      })
    }
  }

  onSave() {
    let trigger: HomeBoxTriggerDto = {};
    trigger.enabled = true;
    trigger.sources = this.triggerSources.map(source => source.value);
    trigger.targets = this.triggerTargets.map(target => target.value);
    trigger.actions = this.triggerActions.map(action => {
      let result = { ...action };
      result.payload = JSON.parse(result.payload);
      return result;
    });
    this.store.addTrigger(trigger, () => this.isVisible = false);
  }

  onCancel() {
    this.isVisible = false;
  }

  onHide() {
    this.store.closeTriggerAddDialog();
  }
}