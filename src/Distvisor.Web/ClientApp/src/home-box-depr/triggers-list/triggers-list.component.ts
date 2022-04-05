import { Component } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { ConfirmationService } from 'primeng/api';
import { HomeBoxState, TriggerVm } from '../state/home-box.state';
import * as DialogActions from '../state/dialogs.actions';
import * as TriggerActions from '../state/triggers.actions';
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
    this.store.dispatch(DialogActions.openTriggerAddDialog());
  }

  onRefreshClicked(): void {
    this.store.dispatch(TriggerActions.loadTriggers());
  }

  onExecuteClicked(trigger: TriggerVm): void {
    this.store.dispatch(TriggerActions.executeTrigger({ triggerId: trigger.id }));
  }

  onTriggerToggled(trigger: TriggerVm, event: any): void {
    this.store.dispatch(TriggerActions.toggleTrigger({ triggerId: trigger.id, enable: event.checked }));
  }

  onDeleteClicked(event: Event, trigger: TriggerVm) {
    this.confirmationService.confirm({
      target: event.target || undefined,
      message: 'Are you sure that you want to delete selected trigger?',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.store.dispatch(TriggerActions.deleteTrigger({ triggerId: trigger.id }));       
      },
      reject: () => {
        //do nothing
      }
    });
  }
}
