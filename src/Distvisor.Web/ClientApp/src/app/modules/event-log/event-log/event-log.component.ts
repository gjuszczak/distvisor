import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { EventLogState } from '../../../root-store/event-log-store/state';
import { selectEventLogEntries } from '../../../root-store/event-log-store/selectors';
import { loadEventLogEntries } from '../../../root-store/event-log-store/actions';
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