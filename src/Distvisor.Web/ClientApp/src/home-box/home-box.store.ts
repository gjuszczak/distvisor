import { Injectable } from '@angular/core';
import { ComponentStore } from '@ngrx/component-store';
import { EMPTY, Observable } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';
import { HomeBoxDeviceDto, HomeBoxTriggerDto, HomeBoxTriggerSourceType } from 'src/api/models';
import { HomeBoxService } from 'src/api/services';

export interface HomeBoxState {
    devices: ReadonlyArray<HomeBoxDeviceDto>;
    triggers: ReadonlyArray<HomeBoxTriggerDto>;
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
        let triggers$ = this.apiClient.apiSecHomeBoxTriggersGet$Json();
        triggers$.pipe(
            catchError(() => EMPTY)
        );
        this.loadTriggers(triggers$);
    }

    reloadDevices() {
        let devices$ = this.apiClient.apiSecHomeBoxDevicesGet$Json();
        devices$.pipe(
            catchError(() => EMPTY)
        );
        this.loadDevices(devices$);
    }

    addTrigger(trigger: HomeBoxTriggerDto, successCallback?: () => void) {
        const triggerToAdd$ = this.apiClient.apiSecHomeBoxTriggersPost({
            body: trigger
        }).pipe(
            map(_ => trigger),
            tap(_ => successCallback && successCallback()),
            catchError(() => EMPTY)
        );
        this.addTriggerLocal(triggerToAdd$);
    };

    deleteTrigger(triggerId: string, successCallback?: () => void) {
        const triggerToDelete$ = this.apiClient.apiSecHomeBoxTriggersIdDelete({
            id: triggerId
        }).pipe(
            map(_ => triggerId),
            tap(_ => successCallback && successCallback()),
            catchError(() => EMPTY)
        );
        this.deleteTriggerLocal(triggerToDelete$);
    }

    toggleTrigger(triggerId: string, enable: boolean, successCallback?: () => void) {
        const triggerToToggle$ = this.apiClient.apiSecHomeBoxTriggersIdTogglePost({
            id: triggerId,
            enable: enable,
        }).pipe(
            map(_ => ({ triggerId, enable })),
            tap(_ => successCallback && successCallback()),
            catchError(() => EMPTY)
        );
        this.toggleTriggerLocal(triggerToToggle$);
    }

    readonly executeTrigger = this.effect((triggerId$: Observable<string>) => {
        return triggerId$.pipe(
            switchMap((triggerId) => this.apiClient.apiSecHomeBoxTriggersIdExecutePost({
                id: triggerId
            }).pipe(
                catchError(() => EMPTY)
            )),
        );
    });

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

    readonly addTriggerLocal = this.updater((state, trigger: HomeBoxTriggerDto) => ({
        ...state,
        triggers: [...state.triggers, trigger],
    }));

    readonly deleteTriggerLocal = this.updater((state, triggerId: string) => ({
        ...state,
        triggers: state.triggers.filter(x => x.id !== triggerId),
    }));

    readonly toggleTriggerLocal = this.updater((state, value: { triggerId: string, enable: boolean }) => ({
        ...state,
        triggers: state.triggers.map(x => x.id === value.triggerId ? { ...x, enabled: value.enable } : x),
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
            enabled: t.enabled,
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