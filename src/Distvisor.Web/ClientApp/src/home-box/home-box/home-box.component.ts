import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { HomeBoxState } from '../state/home-box.state';
import { loadDevices } from '../state/devices.actions';


@Component({
  selector: 'app-home-box',
  templateUrl: './home-box.component.html',
})
export class HomeBoxComponent implements OnInit {  
  constructor(private readonly store: Store<HomeBoxState>) {}

  ngOnInit(): void {
    this.store.dispatch(loadDevices());
  }
}