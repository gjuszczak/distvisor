import { createSelector } from "@ngrx/store";
import { HomeBoxState, NameValue } from "./home-box.state";
import { HomeBoxDeviceDto } from "src/api/models";

export const selectDevices = (state: HomeBoxState) => state.homeBox.devices;

export const selectDevicesShortVm = createSelector(
    selectDevices,
    (devices: readonly HomeBoxDeviceDto[]) => devices.map(d => <NameValue<string>>{
        name: `${d.name} [${d.id}]`,
        value: d.id
    })
);