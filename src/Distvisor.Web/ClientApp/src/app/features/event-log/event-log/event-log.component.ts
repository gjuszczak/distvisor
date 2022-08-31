import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store, Select } from '@ngxs/store';
import { combineLatest, Observable } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';

import 'prismjs/components/prism-json.js';

import { LoadEvents } from '../store/events.actions';
import { EventsState, EventsStateModel } from '../store/events.state';
import { AggregateState, AggregateStateModel } from '../store/aggregate.state';
import { ClearAggregate, LoadAggregate } from '../store/aggregate.actions';
import { distinctUntilChanged, filter, map } from 'rxjs/operators';

@Component({
  selector: 'app-event-log',
  templateUrl: './event-log.component.html',
})
export class EventLogComponent implements OnInit {

  @Select(EventsState.getEvents)
  public readonly events$!: Observable<EventsStateModel>;

  @Select(AggregateState.getAggregate)
  public readonly aggregate$!: Observable<AggregateStateModel>;

  private firstLazyLoad = true;

  constructor(
    private readonly store: Store,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    combineLatest([
      this.route.params,
      this.route.queryParams
    ]).subscribe(([{ aggregateId }, { first, rows }]) => {
      this.reloadEvents(aggregateId, first, rows);
    });

    this.route.params.pipe(
      map(({ aggregateId }) => aggregateId),
      distinctUntilChanged(),
    ).subscribe(aggregateId => {
      if (aggregateId) {
        this.reloadAggregate(aggregateId);
      }
      else {
        this.clearAggregate();
      }
    });
  }

  lazyLoadEvents({ first, rows }: LazyLoadEvent) {
    if (this.firstLazyLoad) {
      this.firstLazyLoad = false;
      return;
    }

    this.router.navigate([], { queryParams: { first, rows } });
  }

  reloadEvents(aggregateId?: string, first?: number, rows?: number) {
    this.store.dispatch(new LoadEvents(aggregateId, first, rows));
  }

  reloadAggregate(aggregateId: string) {
    this.store.dispatch(new LoadAggregate(aggregateId))
  }

  clearAggregate() {
    this.store.dispatch(new ClearAggregate())
  }
}