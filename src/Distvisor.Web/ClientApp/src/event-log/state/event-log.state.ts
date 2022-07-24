import { EventsLogEntryDto } from "src/api/models/events-log-entry-dto";
import { PaginatedList } from "src/shared";

export interface EventLogState {
    eventLog: {
        events: PaginatedList<EventsLogEntryDto>;
    }
}

