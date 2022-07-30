import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { EventLogState } from '../state/event-log.state';
import { selectEventLogEntries } from '../state/entries.selectors';
import { loadEventLogEntries } from '../state/entries.actions';
import { LazyLoadEvent } from 'primeng/api';

import 'prismjs/components/prism-json.js';

@Component({
  selector: 'app-event-log',
  templateUrl: './event-log.component.html',
})
export class EventLogComponent {

  readonly eventLogEntries$ = this.store.select(selectEventLogEntries);
  
  constructor(private readonly store: Store<EventLogState>) {
  }
  
  lazyLoadEventLogEntries(event: LazyLoadEvent) {
    this.store.dispatch(loadEventLogEntries({ firstOffset: event.first, pageSize: event.rows }));
  }
}