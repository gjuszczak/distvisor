import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { State } from '../../../root-store/home-box-store/state';
import { loadDevices } from '../../../root-store/home-box-store/actions';


@Component({
  selector: 'app-home-box',
  templateUrl: './home-box.component.html',
})
export class HomeBoxComponent implements OnInit {  
  constructor(private readonly store: Store<State>) {}

  ngOnInit(): void {
    this.store.dispatch(loadDevices());
  }
}