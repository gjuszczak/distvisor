import { createAction, props } from '@ngrx/store';
import { HomeBoxTriggerDto } from 'src/api/models';

export const loadTriggers = createAction(
    '[Home Box Triggers] Load Triggers'
);

export const triggersLoadedSuccess = createAction(
    '[Home Box Triggers] Triggers Loaded Success',
    props<{ triggers: HomeBoxTriggerDto[] }>()
);

export const addTrigger = createAction(
    '[Home Box Triggers] Add Trigger',
    props<{ trigger: HomeBoxTriggerDto }>()
);

export const triggerAddedSuccess = createAction(
    '[Home Box Triggers] Trigger Added Success',
    props<{ trigger: HomeBoxTriggerDto }>()
);

export const deleteTrigger = createAction(
    '[Home Box Triggers] Delete Trigger',
    props<{ triggerId: string }>()
);

export const triggerDeletedSuccess = createAction(
    '[Home Box Triggers] Trigger Deleted Success',
    props<{ triggerId: string }>()
);

export const toggleTrigger = createAction(
    '[Home Box Triggers] Toggle Trigger',
    props<{ triggerId: string, enable: boolean }>()
);

export const triggerToggledSuccess = createAction(
    '[Home Box Triggers] Trigger Toggled Success',
    props<{ triggerId: string, enable: boolean }>()
);

export const executeTrigger = createAction(
    '[Home Box Triggers] Execute Trigger',
    props<{ triggerId: string }>()
);

export const triggerExecutedSuccess = createAction(
    '[Home Box Triggers] Trigger Executed Success',
    props<{ triggerId: string }>()
);