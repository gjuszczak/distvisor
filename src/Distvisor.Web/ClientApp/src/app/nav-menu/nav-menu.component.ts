import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { MenuItem } from 'primeng/api';
import { NavigationService, INavMenuItems } from '../navigation.service';
import { filter, map, first } from 'rxjs/operators';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html'
})
export class NavMenuComponent implements OnInit, OnDestroy {
  navBrand: string = '';
  menuItems: MenuItem[] = [];
  isAnyMenuItem: boolean = false;
  subscriptions: Subscription[] = [];

  constructor(
    private router: Router,
    private navigationService: NavigationService) { }

  ngOnInit() {
    this.subscriptions.push(
      this.navigationService.getRegisteredNavMenuItems().subscribe(item => {
        const menuItem = <MenuItem>{
          label: item.name,
          icon: item.icon,
          routerLink: item.routerLink,
          visible: false
        };
        if (item.menuVisibile != null) {
          this.subscriptions.push(
            item.menuVisibile.subscribe(visible => {
              menuItem.visible = visible;
              this.isAnyMenuItem = this.menuItems.some(item => item.visible)
            }));
        }
        this.menuItems.push(menuItem);
      }));
    const navigatedMenuLabel = this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(event => event as NavigationEnd),
      map(event => event.url.match('[^?]*')),
      filter(match => match !== null),
      map(match => this.menuItems.findIndex(x => match![0] === x.routerLink)),
      map(index => index >= 0 ? this.menuItems[index].label : "")
    );
    this.subscriptions.push(
      navigatedMenuLabel.subscribe(label => this.navBrand = (label || '')));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }
}
