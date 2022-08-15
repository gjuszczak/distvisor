import { createSelector } from "@ngrx/store";
import { DeviceDetailsVm, DeviceVm, HomeBoxState, NameValue } from "./home-box.state";
import { HomeBoxDeviceDto } from "src/app/api/models";

export const selectDevices = (state: HomeBoxState) => state.homeBox.devices;

export const selectDevicesVm = createSelector(
    selectDevices,
    (devices: readonly HomeBoxDeviceDto[]) => devices.map(d => <DeviceVm>{
        id: d.id,
        header: d.header ?? d.name,
        name: d.name,
        type: d.type,
        location: d.location ?? '---',
        online: d.online ? 'Online' : 'Offline',
    }).sort((a, b) => a.name.localeCompare(b.name))        
);

export const selectDeviceDetailsVmById = (id: string) => createSelector(
    selectDevices,
    (devices: readonly HomeBoxDeviceDto[]) => devices
        .filter(d => d.id === id)
        .map(d => <DeviceDetailsVm>{
            id: d.id,
            header: d.header ?? '',
            name: d.name,
            type: d.type,
            location: d.location ?? '',
            online: d.online ? 'Online' : 'Offline',
            params: JSON.stringify(d.params ?? {}, null, 2)
        })
        .shift()
);

export const selectDevicesShortVm = createSelector(
    selectDevices,
    (devices: readonly HomeBoxDeviceDto[]) => devices.map(d => <NameValue<string>>{
        name: `${d.name} [${d.id}]`,
        value: d.id
    })
);