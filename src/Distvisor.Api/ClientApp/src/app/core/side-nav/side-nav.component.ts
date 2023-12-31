import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { Select } from '@ngxs/store';
import { CoreState, NavigationGroup, UserRole } from '../state/core.state';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html'
})
export class SideNavComponent {

  @Select(CoreState.isNavigationSideBarVisible)
  public readonly isVisible$!: Observable<boolean>;

  @Select(CoreState.userRole)
  public readonly userRole$!: Observable<UserRole | undefined>;

  @Select(CoreState.userNavigationGroups)
  public readonly userNavigationGroups$!: Observable<NavigationGroup[]>;

  public UserRole = UserRole;
}
