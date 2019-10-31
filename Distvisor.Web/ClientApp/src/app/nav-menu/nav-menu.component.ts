import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  items: MenuItem[];

  ngOnInit() {
    this.items = [{
      label: 'Accounting',
      items: [
        { label: 'Tax Calculator', icon: 'pi pi-dollar', routerLink: ['/taxcalc'] }
      ]
    }];
  }
}
