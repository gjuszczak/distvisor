import { Component, OnInit, OnDestroy, Output, EventEmitter, Input, SimpleChanges, OnChanges } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { HomeBoxDeviceDto, HomeBoxTriggerDto, HomeBoxTriggerSourceType } from 'src/api/models';
import { HomeBoxService } from 'src/api/services';
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

  reloadVisualTriggers() {
    // this.visualTriggers = this.triggers.map(t => <VisualTrigger>{
    //   id: t.id,
    //   sources: t.sources?.map(s => `${this.sourceTypeToString(s.type)} [matchParam: ${s.matchParam}]`) || [],
    //   targets: t.targets?.map(t => {
    //     let deviceMatch = this.devices.find(d => d.id === t.deviceIdentifier);
    //     if (deviceMatch) {
    //       return `${deviceMatch.name} [${deviceMatch.id}]`;
    //     }
    //     return t.deviceIdentifier || "";
    //   }),
    //   actions: t.actions?.map(a => JSON.stringify(a, null, 2)) || [],
    // })
  }

  onAddClicked(): void {
    this.store.openTriggerAddDialog();
  }

  onExecuteClicked(trigger: TriggerVm): void {
    // this.subscriptions.push(this.homeBoxService
    //   .apiSecHomeBoxTriggersIdExecutePost({ id: trigger.id })
    //   .subscribe(_ => {
    //     // do nothing
    //   })
    // );
  }

  onDeleteClicked(event: Event, trigger: TriggerVm) {
    // this.confirmationService.confirm({
    //   target: event.target || undefined,
    //   message: 'Are you sure that you want to delete selected trigger?',
    //   icon: 'pi pi-exclamation-triangle',
    //   accept: () => {
    //     this.subscriptions.push(this.homeBoxService
    //       .apiSecHomeBoxTriggersIdDeleteDelete({ id: trigger.id })
    //       .subscribe(_ => {
    //         this.visualTriggers.splice(this.visualTriggers.indexOf(trigger), 1);
    //         this.triggers.splice(this.visualTriggers.findIndex(t => t.id === trigger.id));
    //       })
    //     );
    //   },
    //   reject: () => {
    //     //do nothing
    //   }
    // });

  }
}
