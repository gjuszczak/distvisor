import { Component, OnInit, OnDestroy } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { NavigationService, INavApp } from '../navigation.service';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit, OnDestroy {
  navBrand: string;
  items: MenuItem[] = [];
  subscriptions: Subscription[] = [];

  constructor(
    private navigationService: NavigationService) { }

  ngOnInit() {
    this.subscriptions.push(
      this.navigationService.getRegisteredApps().subscribe(app => {
        this.addApp(app);

        if (app.visibile != null) {
          this.subscriptions.push(
            app.visibile.subscribe(visible => this.toggleApp(app, visible)));
        }
      }));
  }

  addApp(app: INavApp) {
    var ix = this.items.findIndex(x => x === app);
    if (ix < 0) {
      this.items.push({
        label: app.name,
        icon: app.icon,
        routerLink: app.routerLink
      });
    }
  }

  removeApp(app: INavApp) {
    var ix = this.items.findIndex(x => x === app);
    if (ix >= 0) {
      this.items.splice(ix, 1);
    }
  }

  toggleApp(app: INavApp, visible: boolean) {
    if (visible) {
      this.removeApp(app);
    }
    else {
      this.addApp(app);
    }
  }

  onNavBrandUpdate(newNavBrand: string) {
    this.navBrand = newNavBrand;
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());
  }
}
