import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { NavigationService } from '../navigation.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  items: MenuItem[];
  logoutItem: MenuItem;
  navBrand: string;

  constructor(private navigationService: NavigationService) { }

  ngOnInit() {
    this.items = [
      { label: 'Settings', icon: 'pi pi-cog', routerLink: ['/settings'] },
    ];

    this.logoutItem = { label: 'Logout', icon: 'pi pi-sign-out', routerLink: ['/auth/logout'] }
    this.items.push(this.logoutItem);

    this.navigationService.getNavBrand().subscribe(this.onNavBrandUpdate.bind(this));
    this.navigationService.getLogoutVisible().subscribe(this.onLogoutVisibilityUpdate.bind(this));
  }

  onNavBrandUpdate(newNavBrand: string){
    this.navBrand = newNavBrand;
  }

  onLogoutVisibilityUpdate(show: boolean) {
    var logoutIndex = this.items.findIndex((value, _) => value === this.logoutItem)
    if (show && logoutIndex !== -1) {
        this.items.splice(2, 1);
    }
    else if (!show && logoutIndex === -1) {
        this.items.push(this.logoutItem);
    }
  }
}
