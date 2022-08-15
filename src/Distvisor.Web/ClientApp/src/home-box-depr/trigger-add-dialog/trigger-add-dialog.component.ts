import { ChangeDetectorRef, Component } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { MessageService } from 'primeng/api';
import { Observable, Subscription } from 'rxjs';
import { HomeBoxTriggerAction, HomeBoxTriggerDto, HomeBoxTriggerSource, HomeBoxTriggerSourceType, HomeBoxTriggerTarget } from 'src/app/api/models';
import { RfCodeService } from 'src/app/modules/signalr/rfcode.service';
import { HomeBoxState, NameValue } from '../state/home-box.state';
import * as DialogActions from '../state/dialogs.actions';
import * as TriggerActions from '../state/triggers.actions';
import { selectDevicesShortVm } from '../state/devices.selectors';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-trigger-add-dialog',
  templateUrl: './trigger-add-dialog.component.html'
})
export class TriggerAddDialogComponent {
  private syncMatchParamSubscription: Subscription | null = null;

  readonly sourceTypes: NameValue<HomeBoxTriggerSourceType>[] = [
    { name: "RF 433 Receiver", value: HomeBoxTriggerSourceType.Rf433Receiver }
  ]
  readonly actionOnOffOptions: NameValue<boolean | null>[] = [
    { name: "Don't care", value: null },
    { name: "On", value: true },
    { name: "Off", value: false }
  ]
  readonly targetOptions$: Observable<NameValue<string>[]> = this.store.pipe(
    select(selectDevicesShortVm),
    tap(t => {
      if (!t || t.length === 0) {
        this.selectedTarget = null;
      }
      else if (!this.selectedTarget) {
        this.selectedTarget = t[0];
      }
    }));

  selectedSourceType: NameValue<HomeBoxTriggerSourceType> = this.sourceTypes[0];
  selectedSourceMatchParam: string = "";
  selectedTriggerAction: number = 0;
  selectedTarget: NameValue<string> | null = null;
  triggerSources: NameValue<HomeBoxTriggerSource>[] = [];
  triggerTargets: NameValue<HomeBoxTriggerTarget>[] = [];
  triggerActions: HomeBoxTriggerAction[] = [this.emptyAction()];
  isSyncMatchParamInProgress: boolean = false;
  isVisible: boolean = true;

  constructor(
    private rfCodeService: RfCodeService,
    private messageService: MessageService,
    private cdref: ChangeDetectorRef,
    private store: Store<HomeBoxState>) {
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

    if (trigger.sources?.length < 1) {
      this.messageService.add({
        severity: 'error',
        summary: "Validation failed",
        detail: "Please provide at least one trigger source",
      });
      return;
    }

    if (trigger.targets?.length < 1) {
      this.messageService.add({
        severity: 'error',
        summary: "Validation failed",
        detail: "Please provide at least one trigger target",
      });
      return;
    }

    if (trigger.actions?.length < 1) {
      this.messageService.add({
        severity: 'error',
        summary: "Validation failed",
        detail: "Please provide at least one trigger action",
      });
      return;
    }

    this.store.dispatch(TriggerActions.addTrigger({ trigger }));
  }

  onCancel() {
    this.isVisible = false;
  }

  onHide() {
    this.store.dispatch(DialogActions.closeTriggerAddDialog());
  }
}