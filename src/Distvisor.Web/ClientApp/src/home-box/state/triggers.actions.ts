import { createAction, props } from '@ngrx/store';
import { HomeBoxTriggerDto } from 'src/api/models';

export const loadTriggers = createAction(
    '[Home Box Triggers] Load Triggers'
);

export const triggersLoadedSuccess = createAction(
    '[Home Box Triggers] Triggers Loaded Success',
    props<{ triggers: HomeBoxTriggerDto[] }>()
);