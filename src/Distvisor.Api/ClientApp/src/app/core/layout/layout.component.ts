import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { Select } from '@ngxs/store';

import { CoreState } from '../state/core.state';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
})
export class LayoutComponent {  
  @Select(CoreState.isNavigationSideBarVisible)
  public readonly isNavigationSideBarVisible$!: Observable<boolean>;
}
