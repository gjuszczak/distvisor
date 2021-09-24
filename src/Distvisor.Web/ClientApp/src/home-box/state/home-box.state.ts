import { HomeBoxDeviceDto, HomeBoxTriggerDto } from "src/api/models";

export interface HomeBoxState {
    homeBox: {
        devices: ReadonlyArray<HomeBoxDeviceDto>;
        triggers: ReadonlyArray<HomeBoxTriggerDto>;
        dialogs: DialogsState;
    }
}

export interface DialogsState {
    isTriggerAddDialogOpened: boolean;
}

export interface TriggerVm {
    id: string;
    enabled: boolean;
    sources: string[];
    targets: string[];
    actions: string[];
}

export interface NameValue<T> {
    name: string;
    value: T;
}