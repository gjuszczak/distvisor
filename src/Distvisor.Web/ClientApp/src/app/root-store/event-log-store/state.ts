import { EventsLogEntryDto } from "src/app/api/models/events-log-entry-dto";
import { PaginatedList } from "src/app/shared";

export interface State {
    entries: PaginatedList<EventsLogEntryDto>;
}
