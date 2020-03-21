import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { MenuItem } from 'primeng/api';
import { NavigationService, INavApp } from '../navigation.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit, OnDestroy {
  navBrand: string;
  availableApps: INavApp[] = [];
  menuItems: MenuItem[] = [];
  isAnyMenuItem: boolean = false;
  subscriptions: Subscription[] = [];

  constructor(
    private router: Router,
    private navigationService: NavigationService) { }

  ngOnInit() {
    this.subscriptions.push(
      this.navigationService.getRegisteredApps().subscribe(app => {
        this.availableApps.push(app);
        if (app.menuVisibile != null) {
          this.subscriptions.push(
            app.menuVisibile.subscribe(visible => this.toggleAppMenuVisibility(app, visible)));
        }
      }));

    this.subscriptions.push(
      this.router.events.subscribe(event => {
        if (event instanceof NavigationEnd) {
          var ix = this.availableApps.findIndex(x => event.url.match('[^?]*')[0] === x.routerLink);
          if (ix >= 0) {
            this.navBrand = this.availableApps[ix].name;
          }
          else {
            this.navBrand = "";
          }
        }
      }));
  }

  toggleAppMenuVisibility(app: INavApp, visible: boolean) {
    if (visible) {
      var ix = this.menuItems.findIndex(x => x.label === app.name);
      if (ix < 0) {
        this.menuItems.push({
          label: app.name,
          icon: app.icon,
          routerLink: app.routerLink
        });
      }
    }
    else {
      var ix = this.menuItems.findIndex(x => x.label === app.name);
      if (ix >= 0) {
        this.menuItems.splice(ix, 1);
      }
    }

    this.isAnyMenuItem = (this.menuItems.length > 0);
  }

  onNavBrandUpdate(newNavBrand: string) {
    this.navBrand = newNavBrand;
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }
}
