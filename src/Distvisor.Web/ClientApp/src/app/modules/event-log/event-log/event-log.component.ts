import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { LazyLoadEvent } from 'primeng/api';

import 'prismjs/components/prism-json.js';
import { EventLogStoreActions, EventLogStoreSelectors, RootStoreState } from 'src/app/root-store';

@Component({
  selector: 'app-event-log',
  templateUrl: './event-log.component.html',
})
export class EventLogComponent {

  readonly eventLogEntries$ = this.store.select(EventLogStoreSelectors.selectEventLogEntries);
  
  constructor(private readonly store: Store<RootStoreState.State>) {
  }
  
  lazyLoadEventLogEntries(event: LazyLoadEvent) {
    this.store.dispatch(EventLogStoreActions.loadEventLogEntries({ firstOffset: event.first, pageSize: event.rows }));
  }
}