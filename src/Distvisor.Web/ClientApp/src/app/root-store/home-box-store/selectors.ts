import { createFeatureSelector, createSelector } from "@ngrx/store";
import { DeviceDetailsVm, DeviceVm, State } from "./state";
import { DeviceDto } from "src/app/api/models";
import { NameValue } from "src/app/shared";

export const selectHomeBoxState =  createFeatureSelector<State>('homeBox');

export const selectDevices = createSelector(
  selectHomeBoxState,
  homeBox => homeBox.devices
);

export const selectDevicesVm = createSelector(
    selectDevices,
    (devices: readonly DeviceDto[]) => devices.map(d => <DeviceVm>{
        id: d.id,
        header: d.header ?? d.name,
        name: d.name,
        type: d.type,
        location: d.location ?? '---',
        isOnline: d.isOnline ? 'Online' : 'Offline',
    }).sort((a, b) => a.name.localeCompare(b.name))        
);

export const selectDeviceDetailsVmById = (id: string) => createSelector(
    selectDevices,
    (devices: readonly DeviceDto[]) => devices
        .filter(d => d.id === id)
        .map(d => <DeviceDetailsVm>{
            id: d.id,
            header: d.header ?? '',
            name: d.name,
            type: d.type,
            location: d.location ?? '',
            isOnline: d.isOnline ? 'Online' : 'Offline',
            params: JSON.stringify(d.params ?? {}, null, 2)
        })
        .shift()
);

export const selectDevicesShortVm = createSelector(
    selectDevices,
    (devices: readonly DeviceDto[]) => devices.map(d => <NameValue<string>>{
        name: `${d.name} [${d.id}]`,
        value: d.id
    })
);

export const selectDialogs = createSelector(
  selectHomeBoxState,
  homeBox => homeBox.dialogs
);

export const selectIsTriggerAddDialogOpened = createSelector(
  selectDialogs,
  dialogs => dialogs.isTriggerAddDialogOpened
);

export const selectIsDeviceDetailsDialogOpened = createSelector(
  selectDialogs,
  dialogs => dialogs.isDeviceDetailsDialogOpened
);

export const selectDeviceDetailsDialogParam = createSelector(
  selectDialogs,
  dialogs => dialogs.deviceDetailsDialogParam
);