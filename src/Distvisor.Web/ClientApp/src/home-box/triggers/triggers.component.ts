import { Component, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs';

interface Trigger {
  guid: string;
  source: string;
  target: string;
  params: any;
};

@Component({
  selector: 'app-triggers',
  templateUrl: './triggers.component.html',
})
export class TriggersComponent implements OnInit, OnDestroy {

  @Output() onAdd: EventEmitter<any> = new EventEmitter();

  private subscriptions: Subscription[] = [];
  triggers: Trigger[] = [];

  constructor() {
  }

  ngOnInit() {
    this.reloadList();
  }

  reloadList() {
    this.triggers = [
      {
        guid: "1",
        source: "RF433 Receiver [code:1233345]",
        target: "Test device 123",
        params: { switch: 'toggle', r: 255 },
      }
    ]
  }

  onAddClicked(): void {
    this.onAdd.emit();
  }

  onExecuteClicked(): void {

  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}
