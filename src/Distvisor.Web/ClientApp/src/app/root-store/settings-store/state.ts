import { GatewaySessionDto } from "src/app/api/models";
import { PaginatedList } from "src/app/shared";

export interface State {
    homeBox: HomeBoxSettingsState;
}

export interface HomeBoxSettingsState {
    gatewaySessions: PaginatedList<GatewaySessionDto>;
}