import { Component, OnInit } from '@angular/core';
import { select, Store } from '@ngrx/store';
import { selectIsDeviceDetailsDialogOpened, selectIsTriggerAddDialogOpened } from '../state/dialogs.selectors';
import { HomeBoxState } from '../state/home-box.state';
import { loadTriggers } from '../state/triggers.actions';
import { loadDevices } from '../state/devices.actions';


@Component({
  selector: 'app-home-box',
  templateUrl: './home-box.component.html',
})
export class HomeBoxComponent implements OnInit {  
  readonly isTriggerAddDialogOpened$ = this.store.pipe(select(selectIsTriggerAddDialogOpened));
  readonly isDeviceDetailsDialogOpened$ = this.store.pipe(select(selectIsDeviceDetailsDialogOpened));

  constructor(private readonly store: Store<HomeBoxState>) {}

  ngOnInit(): void {
    this.store.dispatch(loadTriggers());
    this.store.dispatch(loadDevices());
  }
}