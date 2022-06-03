export interface SettingsState {
    settings: {
        homeBox: HomeBoxSettingsState;
    }
}

export interface HomeBoxSettingsState {
    sessions: ReadonlyArray<string>;
}