import { EventsLogEntryDto } from "src/app/api/models/events-log-entry-dto";
import { PaginatedList } from "src/app/shared";

export interface EventLogState {
    eventLog: {
        entries: PaginatedList<EventsLogEntryDto>;
    }
}

