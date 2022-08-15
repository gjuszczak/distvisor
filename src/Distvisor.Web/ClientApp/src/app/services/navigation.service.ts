import { Subject, Observable, ReplaySubject } from "rxjs";
import { Injectable } from "@angular/core";

export interface INavMenuItems {
  name: string;
  icon: string | null;
  routerLink: string;
  menuVisibile?: Observable<boolean> | null;
}

@Injectable()
export class NavigationService {
  private apps = new ReplaySubject<INavMenuItems>();

  registerNavMenuItem(app: INavMenuItems) {
    this.apps.next(app);
  }

  getRegisteredNavMenuItems(): Observable<INavMenuItems> {
    return this.apps;
  }
}
