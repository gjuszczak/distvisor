import { Component } from '@angular/core';
import { Store, Select } from '@ngxs/store';
import { Observable } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';

import 'prismjs/components/prism-json.js';

import { EventLogState, EventLogStateModel } from '../store/event-log.state';
import { LoadEventLog } from '../store/event-log.actions';

@Component({
  selector: 'app-event-log',
  templateUrl: './event-log.component.html',
})
export class EventLogComponent {

  @Select(EventLogState.getEventLog) readonly eventLog$!: Observable<EventLogStateModel>;
  
  constructor(private readonly store: Store) {
  }
  
  lazyLoadEventLog({ first, rows }: LazyLoadEvent) {
    this.store.dispatch(new LoadEventLog(first, rows));
  }
}