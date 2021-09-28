import { createSelector } from '@ngrx/store';
import { HomeBoxState } from './home-box.state';


export const selectDialogs = (state: HomeBoxState) => state.homeBox.dialogs;

export const selectIsTriggerAddDialogOpened = createSelector(
  selectDialogs,
  dialogs => dialogs.isTriggerAddDialogOpened
)