import { Component, OnInit, OnDestroy } from '@angular/core';
import { EventLogService } from 'src/api/services';
import { EventLogDto } from 'src/api/models';
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

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}
