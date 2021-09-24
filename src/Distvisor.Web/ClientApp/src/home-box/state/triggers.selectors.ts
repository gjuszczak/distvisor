import { createSelector } from "@ngrx/store";
import { HomeBoxState, TriggerVm } from "./home-box.state";
import { selectDevices } from "./devices.selectors";
import { HomeBoxTriggerSourceType } from "src/api/models";

export const selectTriggers = (state: HomeBoxState) => state.homeBox.triggers;

export const selectTriggersVm = createSelector(
    selectDevices,
    selectTriggers,
    (devices, triggers) => triggers.map(t => <TriggerVm>{
        id: t.id,
        enabled: t.enabled,
        sources: t.sources?.map(s => `${sourceTypeToString(s.type)} [matchParam: ${s.matchParam}]`) || [],
        targets: t.targets?.map(t => {
            let deviceMatch = devices.find(d => d.id === t.deviceIdentifier);
            if (deviceMatch) {
                return `${deviceMatch.name} [${deviceMatch.id}]`;
            }
            return t.deviceIdentifier || "";
        }),
        actions: t.actions?.map(a => JSON.stringify(a, null, 2)) || [],
    })
);

const sourceTypeToString = (type?: HomeBoxTriggerSourceType): string | undefined => {
    if (type === HomeBoxTriggerSourceType.Rf433Receiver) {
        return "RF 433 Receiver";
    }
    return type;
}