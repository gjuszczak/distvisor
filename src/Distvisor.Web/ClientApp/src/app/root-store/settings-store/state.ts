import { GatewaySessionDto } from "src/app/api/models";
import { PaginatedList } from "src/app/shared";

export interface SettingsState {
    settings: {
        homeBox: HomeBoxSettingsState;
    }
}

export interface HomeBoxSettingsState {
    gatewaySessions: PaginatedList<GatewaySessionDto>;
}