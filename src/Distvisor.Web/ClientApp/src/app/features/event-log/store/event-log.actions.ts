import { EventsLogEntryDtoPaginatedList } from "src/app/api/models";

export class LoadEventLog {
    static readonly type = '[Event Log] Load Event Log';
    constructor(
        public readonly firstOffset?: number,
        public readonly pageSize?: number
    ) { }
}

export class LoadEventLogSuccess {
    static readonly type = '[Event Log] Load Event Log Success';
    constructor(
        public readonly eventLogList: EventsLogEntryDtoPaginatedList 
    ) { }
}