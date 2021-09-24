import { Component } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { ConfirmationService } from 'primeng/api';
import { HomeBoxState, TriggerVm } from '../state/home-box.state';
import { selectDevices } from '../state/devices.selectors';
import { selectTriggers, selectTriggersVm } from '../state/triggers.selectors';

@Component({
  selector: 'app-triggers-list',
  templateUrl: './triggers-list.component.html',
})
export class TriggersListComponent {
  readonly devices$ = this.store.pipe(select(selectDevices));
  readonly triggers$ = this.store.pipe(select(selectTriggers));
  readonly triggersVm$ = this.store.pipe(select(selectTriggersVm));

  constructor(
    private store: Store<HomeBoxState>,
    private confirmationService: ConfirmationService) {
  }

  onAddClicked(): void {
    //this.store.openTriggerAddDialog();
  }

  onRefreshClicked(): void {
    //this.store.reloadTriggers();
  }

  onExecuteClicked(trigger: TriggerVm): void {
    //this.store.executeTrigger(trigger.id);
  }

  onTriggerToggled(trigger: TriggerVm, event: any): void {
    //this.store.toggleTrigger(trigger.id, event.checked);
  }

  onDeleteClicked(event: Event, trigger: TriggerVm) {
    this.confirmationService.confirm({
      target: event.target || undefined,
      message: 'Are you sure that you want to delete selected trigger?',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        //this.store.deleteTrigger(trigger.id);
      },
      reject: () => {
        //do nothing
      }
    });
  }
}
