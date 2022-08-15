import { EventLogStoreState } from "./event-log-store";
import { HomeBoxStoreState } from "./home-box-store";
import { SettingsStoreState } from "./settings-store";

export interface State {
    settings: SettingsStoreState.SettingsState;
    eventLog: EventLogStoreState.EventLogState;
    homeBox: HomeBoxStoreState.HomeBoxState;
}