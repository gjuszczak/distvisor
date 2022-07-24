import { GatewaySessionDto } from "src/api/models";
import { PaginatedList } from "src/shared/paginated-list.model";

export interface SettingsState {
    settings: {
        homeBox: HomeBoxSettingsState;
    }
}

export interface HomeBoxSettingsState {
    gatewaySessions: PaginatedList<GatewaySessionDto>;
}