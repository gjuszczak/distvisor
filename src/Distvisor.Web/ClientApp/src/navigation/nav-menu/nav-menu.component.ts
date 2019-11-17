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
  navBrand: string;

  constructor(private navigationService: NavigationService) { }

  ngOnInit() {
    this.items = [{
      label: 'Account',
      items: [
        { label: 'Fetch data', icon: 'pi pi-dollar', routerLink: ['/fetch-data'] },
        { label: 'Logout', routerLink: ['/authentication/logout'] }
      ]
    }];

    this.navigationService.getNavBrand().subscribe(this.onNavBrandUpdate.bind(this));
  }

  onNavBrandUpdate(newNavBrand: string){
    this.navBrand = newNavBrand;
  }
}
