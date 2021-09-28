import { createAction, props } from '@ngrx/store';

export const openTriggerAddDialog = createAction(
    '[Home Box Dialogs] Open Trigger Add dialog'
);

export const closeTriggerAddDialog = createAction(
    '[Home Box Dialogs] Close Trigger Add dialog'
);