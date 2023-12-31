import { Component, Input } from '@angular/core';
import { Store } from '@ngxs/store';
import { ToggleNavigationSideBar } from '../state/core.actions';

@Component({
  selector: 'app-brand',
  templateUrl: './brand.component.html'
})
export class BrandComponent {

  @Input() withSeparator: boolean = false;

  constructor(
    private readonly store: Store,
  ) { }

  toggleSideNav() {
    this.store.dispatch(new ToggleNavigationSideBar());
  }
}
