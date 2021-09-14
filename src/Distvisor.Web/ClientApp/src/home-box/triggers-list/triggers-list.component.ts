import { Component } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { HomeBoxStore, TriggerVm } from '../home-box.store';

@Component({
  selector: 'app-triggers-list',
  templateUrl: './triggers-list.component.html',
})
export class TriggersListComponent {
  readonly triggers$ = this.store.triggers$
  readonly devices$ = this.store.devices$;
  readonly triggersVm$ = this.store.triggersVm$;

  constructor(
    private store: HomeBoxStore,
    private confirmationService: ConfirmationService) {
  }

  onAddClicked(): void {
    this.store.openTriggerAddDialog();
  }

  onRefreshClicked(): void {
    this.store.reloadTriggers();
  }

  onExecuteClicked(trigger: TriggerVm): void {
    this.store.executeTrigger(trigger.id);
  }

  onTriggerToggled(trigger: TriggerVm, event: any): void {
    this.store.toggleTrigger(trigger.id, event.checked);
  }

  onDeleteClicked(event: Event, trigger: TriggerVm) {
    this.confirmationService.confirm({
      target: event.target || undefined,
      message: 'Are you sure that you want to delete selected trigger?',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.store.deleteTrigger(trigger.id);
      },
      reject: () => {
        //do nothing
      }
    });
  }
}
