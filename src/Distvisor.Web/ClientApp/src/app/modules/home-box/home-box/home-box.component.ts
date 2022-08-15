import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { HomeBoxState } from '../../../root-store/home-box-store/state';
import { loadDevices } from '../../../root-store/home-box-store/actions';


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