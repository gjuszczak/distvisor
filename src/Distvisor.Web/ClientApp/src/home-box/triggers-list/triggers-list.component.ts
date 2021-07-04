import { Component, OnInit, OnDestroy, Output, EventEmitter, Input, SimpleChanges, OnChanges } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { DeviceDto, HomeBoxTriggerDto, HomeBoxTriggerSourceType } from 'src/api/models';
import { HomeBoxService } from 'src/api/services';

interface VisualTrigger {
  id: string,
  sources: string[],
  targets: string[],
  actions: string[]
}

@Component({
  selector: 'app-triggers-list',
  templateUrl: './triggers-list.component.html',
})
export class TriggersListComponent implements OnInit, OnChanges, OnDestroy {
  @Input() devices: DeviceDto[] = [];
  @Output() onAdd: EventEmitter<any> = new EventEmitter();

  private subscriptions: Subscription[] = [];
  triggers: HomeBoxTriggerDto[] = [];
  visualTriggers: VisualTrigger[] = [];

  constructor(private homeBoxService: HomeBoxService, private confirmationService: ConfirmationService) {
  }

  ngOnInit() {
    this.reloadList();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['devices']) {
      this.reloadVisualTriggers();
    }
  }

  reloadList() {
    this.subscriptions.push(
      this.homeBoxService.apiSecHomeBoxTriggersListGet$Json()
        .subscribe(triggers => {
          this.triggers = triggers;
          this.reloadVisualTriggers();
        }));
  }

  reloadVisualTriggers() {
    this.visualTriggers = this.triggers.map(t => <VisualTrigger>{
      id: t.id,
      sources: t.sources?.map(s => `${this.sourceTypeToString(s.type)} [matchParam: ${s.matchParam}]`) || [],
      targets: t.targets?.map(t => {
        let deviceMatch = this.devices.find(d => d.identifier === t.deviceIdentifier);
        if (deviceMatch) {
          return `${deviceMatch.name} [${deviceMatch.identifier}]`;
        }
        return t.deviceIdentifier || "";
      }),
      actions: t.actions?.map(a => JSON.stringify(a, null, 2)) || [],
    })
  }

  sourceTypeToString(type?: HomeBoxTriggerSourceType) {
    if (type === HomeBoxTriggerSourceType.Rf433Receiver) {
      return "RF 433 Receiver";
    }
    return type;
  }

  onAddClicked(): void {
    this.onAdd.emit();
  }

  onExecuteClicked(trigger: VisualTrigger): void {
    this.subscriptions.push(this.homeBoxService
      .apiSecHomeBoxTriggersIdExecutePost({ id: trigger.id })
      .subscribe(_ => {
        // do nothing
      })
    );
  }

  onDeleteClicked(event: Event, trigger: VisualTrigger) {
    this.confirmationService.confirm({
      target: event.target || undefined,
      message: 'Are you sure that you want to delete selected trigger?',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.subscriptions.push(this.homeBoxService
          .apiSecHomeBoxTriggersIdDeleteDelete({ id: trigger.id })
          .subscribe(_ => {
            this.visualTriggers.splice(this.visualTriggers.indexOf(trigger), 1);
            this.triggers.splice(this.visualTriggers.findIndex(t => t.id === trigger.id));
          })
        );
      },
      reject: () => {
          //do nothing
      }
  });

  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}
