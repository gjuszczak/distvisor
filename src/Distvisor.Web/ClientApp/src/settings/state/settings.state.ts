import { GatewaySessionDto } from "src/api/models";

export interface SettingsState {
    settings: {
        homeBox: HomeBoxSettingsState;
    }
}

export interface HomeBoxSettingsState {
    gatewaySessions: PaginatedList<GatewaySessionDto>;
}

export interface PaginatedList<T> {
    items: T[],
    firstOffset: number,
    pageSize: number,
    pageSizeOptions: number[]
    totalCount: number,
    loading: boolean
}