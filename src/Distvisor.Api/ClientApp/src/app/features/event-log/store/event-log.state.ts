import { Injectable } from "@angular/core";
import { State } from "@ngxs/store";
import { AggregateState } from "./aggregate.state";

import { EventsState } from "./events.state";

export interface EventLogStateModel {
};

export const eventLogStateDefaults: EventLogStateModel = {
};

@State<EventLogStateModel>({
    name: 'eventLog',
    defaults: eventLogStateDefaults,
    children: [EventsState, AggregateState]
})
@Injectable()
export class EventLogState {
}