import { DeviceDto, DeviceType } from "src/app/api/models";

export interface State {
    devices: ReadonlyArray<DeviceDto>;
    dialogs: DialogsState;
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
    type: DeviceType;
    location: string;
    isOnline: string;
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
