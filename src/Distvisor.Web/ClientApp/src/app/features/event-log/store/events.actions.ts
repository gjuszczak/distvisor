import { EventsListDto} from "src/app/api/models";

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
        public readonly error: any 
    ) { }
}