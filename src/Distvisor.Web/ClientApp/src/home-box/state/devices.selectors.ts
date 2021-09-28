import { createSelector } from "@ngrx/store";
import { DeviceVm, HomeBoxState, NameValue } from "./home-box.state";
import { HomeBoxDeviceDto } from "src/api/models";

export const selectDevices = (state: HomeBoxState) => state.homeBox.devices;

export const selectDevicesVm = createSelector(
    selectDevices,
    (devices: readonly HomeBoxDeviceDto[]) => devices.map(d => <DeviceVm>{
        id: d.id,
        name: d.name,
        type: d.type,
        location: d.location || '---',
        online: d.online ? 'Online': 'Offline',
    })
);

export const selectDevicesShortVm = createSelector(
    selectDevices,
    (devices: readonly HomeBoxDeviceDto[]) => devices.map(d => <NameValue<string>>{
        name: `${d.name} [${d.id}]`,
        value: d.id
    })
);