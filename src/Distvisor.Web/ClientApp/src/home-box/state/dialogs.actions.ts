import { createAction, props } from '@ngrx/store';

export const openTriggerAddDialog = createAction(
    '[Home Box Dialogs] Open Trigger Add dialog'
);

export const closeTriggerAddDialog = createAction(
    '[Home Box Dialogs] Close Trigger Add dialog'
);

export const openDeviceDetailsDialog = createAction(
    '[Home Box Dialogs] Open Device Details dialog',
    props<{ deviceId: string }>()
);

export const closeDeviceDetailsDialog = createAction(
    '[Home Box Dialogs] Close Device Details dialog'
);