import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HomeBoxDeviceDto, HomeBoxTriggerDto, HomeBoxTriggerSourceType } from 'src/api/models';
import { HomeBoxService } from 'src/api/services';

export interface HomeBoxState {
    devices: ReadonlyArray<HomeBoxDeviceDto>;
    triggers: ReadonlyArray<HomeBoxTriggerDto>;
    isTriggerAddDialogOpened: boolean;
}

export interface TriggerVm {
    id: string;
    sources: string[];
    targets: string[];
    actions: string[];
}

export interface NameValue<T> {
    name: string;
    value: T;
}

@Injectable()
export class HomeBoxStore extends ComponentStore<HomeBoxState> {
    constructor(private readonly apiClient: HomeBoxService) {
        super({
            devices: [],
            triggers: [],
            isTriggerAddDialogOpened: false
        });
    }

    reloadTriggers() {
        let triggers$ = this.apiClient.apiSecHomeBoxTriggersListGet$Json();
        this.loadTriggers(triggers$);
    }

    reloadDevices() {
        let devices$ = this.apiClient.apiSecHomeBoxDevicesGet$Json();
        this.loadDevices(devices$);
    }

    storeTrigger(trigger: HomeBoxTriggerDto) {
        let triggerToAdd$ = this.apiClient.apiSecHomeBoxTriggersAddPost({
            body: trigger
        }).pipe(
            map(_ => trigger)
        );
        this.addTrigger(triggerToAdd$);
    };

    readonly loadTriggers = this.updater((state, triggers: HomeBoxTriggerDto[]) => ({
        ...state,
        triggers,
    }));

    readonly loadDevices = this.updater((state, devices: HomeBoxDeviceDto[]) => ({
        ...state,
        devices,
    }));

    readonly openTriggerAddDialog = this.updater((state) => ({
        ...state,
        isTriggerAddDialogOpened: true
    }));

    readonly closeTriggerAddDialog = this.updater((state) => ({
        ...state,
        isTriggerAddDialogOpened: false
    }));

    readonly addTrigger = this.updater((state, trigger: HomeBoxTriggerDto) => ({
        ...state,
        triggers: [...state.triggers, trigger],
    }));

    readonly devices$ = this.select(state => state.devices);

    readonly devicesShortVm$ = this.select(
        this.devices$,
        devices => devices.map(d => <NameValue<string>>{
            name: `${d.name} [${d.id}]`,
            value: d.id
        })
    );

    readonly triggers$ = this.select(state => state.triggers);

    readonly triggersVm$ = this.select(
        this.devices$,
        this.triggers$,
        (devices, triggers) => triggers.map(t => <TriggerVm>{
            id: t.id,
            sources: t.sources?.map(s => `${this.sourceTypeToString(s.type)} [matchParam: ${s.matchParam}]`) || [],
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

    readonly isTriggerAddDialogOpened$ = this.select(state => state.isTriggerAddDialogOpened);

    private sourceTypeToString(type?: HomeBoxTriggerSourceType): string | undefined {
        if (type === HomeBoxTriggerSourceType.Rf433Receiver) {
            return "RF 433 Receiver";
        }
        return type;
    }
}