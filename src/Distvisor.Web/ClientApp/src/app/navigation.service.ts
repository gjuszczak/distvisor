import { Subject, Observable, ReplaySubject } from "rxjs";
import { Injectable } from "@angular/core";

export interface INavApp {
  name: string;
  icon: string | null;
  routerLink: string;
  menuVisibile?: Observable<boolean> | null;
}

@Injectable()
export class NavigationService {
  private apps = new ReplaySubject<INavApp>();

  registerApp(app: INavApp) {
    this.apps.next(app);
  }

  getRegisteredApps(): Observable<INavApp> {
    return this.apps;
  }
}
