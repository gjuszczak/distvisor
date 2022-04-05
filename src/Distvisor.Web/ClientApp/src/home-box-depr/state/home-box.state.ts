import { HomeBoxDeviceDto, HomeBoxDeviceType, HomeBoxTriggerDto } from "src/api/models";

export interface HomeBoxState {
    homeBox: {
        devices: ReadonlyArray<HomeBoxDeviceDto>;
        triggers: ReadonlyArray<HomeBoxTriggerDto>;
        dialogs: DialogsState;
    }
}

export interface DialogsState {
    isTriggerAddDialogOpened: boolean;
    isDeviceDetailsDialogOpened: boolean;
    deviceDetailsDialogParam: {
        deviceId: string;
    };
}

export interface DeviceVm {
    id: string;
    header: string;
    name: string;
    type: HomeBoxDeviceType;
    location: string;
    online: string;
}

export interface DeviceDetailsVm extends DeviceVm {
    params: string;
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