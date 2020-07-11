import { Component, OnInit, OnDestroy } from '@angular/core';
import { EventLogService } from 'src/api/services';
import { EventLogDto } from 'src/api/models';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-event-log',
  templateUrl: './event-log.component.html'
})
export class EventLogComponent implements OnInit, OnDestroy {

  private subscriptions: Subscription[] = [];
  events: EventLogDto[];

  constructor(private eventLogService: EventLogService) {
  }

  ngOnInit() {
    this.reloadList();
  }

  reloadList() {
    this.subscriptions.push(
      this.eventLogService.apiEventLogListGet$Json()
        .subscribe(events => {
          this.events = events;
        }));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}