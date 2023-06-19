import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store, Select } from '@ngxs/store';
import { combineLatest, Observable } from 'rxjs';
import { LazyLoadEvent, MenuItem } from 'primeng/api';

import 'prismjs/components/prism-json.js';

import { HideEventsModal, LoadEvents, ReplayEvents, ShowEventsModal } from '../store/events.actions';
import { EventsList, EventsModal, EventsModals, EventsState } from '../store/events.state';
import { AggregateState, AggregateStateModel } from '../store/aggregate.state';
import { ClearAggregate, LoadAggregate } from '../store/aggregate.actions';
import { distinctUntilChanged, map } from 'rxjs/operators';

@Component({
  selector: 'app-event-log',
  templateUrl: './event-log.component.html',
})
export class EventLogComponent implements OnInit {
  
  @Select(EventsState.getModal)
  public readonly modal$!: Observable<EventsModal>;

  @Select(EventsState.getEvents)
  public readonly events$!: Observable<EventsList>;

  @Select(AggregateState.getAggregate)
  public readonly aggregate$!: Observable<AggregateStateModel>;

  public readonly EventsModals = EventsModals;

  public eventLogMenuItems: MenuItem[] = [];
  private firstLazyLoad = true;

  constructor(
    private readonly store: Store,
    private readonly router: Router,
    private readonly route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.eventLogMenuItems = [
      {
        label: 'Replay events to RS',
        icon: 'pi pi-history',
        command: () => {
          this.store.dispatch(new ShowEventsModal(EventsModals.ReplayEvents));
        }
      }
    ];

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

  hideEventsModal() {
    this.store.dispatch(new HideEventsModal());
  }

  reloadEvents(aggregateId?: string, first?: number, rows?: number) {
    this.store.dispatch(new LoadEvents(aggregateId, first, rows));
  }

  reloadAggregate(aggregateId: string) {
    this.store.dispatch(new LoadAggregate(aggregateId));
  }

  clearAggregate() {
    this.store.dispatch(new ClearAggregate());
  }

  replayEvents() {
    this.store.dispatch(new ReplayEvents());
  }
}