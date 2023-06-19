import { EventsListDto} from "src/app/api/models";
import { EventsModals } from "./events.state";

export class LoadEvents {
    static readonly type = '[Event Log] LoadEvents';
    constructor(
        public readonly aggregateId?: string,
        public readonly first?: number,
        public readonly rows?: number
    ) { }
}

export class LoadEventsSuccess {
    static readonly type = '[Event Log] LoadEventsSuccess';
    constructor(
        public readonly eventsList: EventsListDto 
    ) { }
}

export class LoadEventsFail {
    static readonly type = '[Event Log] LoadEventsFail';
    constructor(
        public readonly error: string 
    ) { }
}

export class ShowEventsModal {
    static readonly type = '[Event Log] ShowEventsModal';
    constructor ( 
        public readonly modal: EventsModals,
    ) { }
}

export class HideEventsModal {
    static readonly type = '[Event Log] HideEventsModal';
    constructor (
        public readonly reloadEvents: boolean = false
    ) { }
}

export class FailEventsModal {
    static readonly type = '[Event Log] FailEventsModal';
    constructor (
        public readonly error: string
    ) { }
}

export class ReplayEvents {
    static readonly type = '[Event Log] ReplayEvents';
}