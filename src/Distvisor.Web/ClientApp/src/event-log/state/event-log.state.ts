import { EventsLogEntryDto } from "src/api/models/events-log-entry-dto";
import { PaginatedList } from "src/app-common";

export interface EventLogState {
    eventLog: {
        entries: PaginatedList<EventsLogEntryDto>;
    }
}

