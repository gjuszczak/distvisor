import { Subject, Observable, ReplaySubject } from "rxjs";

export interface INavApp {
  name: string;
  icon: string | null;
  routerLink: string;
  visibile?: Observable<boolean> | null;
}

export class NavigationService {
  private apps = new ReplaySubject<INavApp>();

  registerApp(app: INavApp) {
    this.apps.next(app);
  }

  getRegisteredApps(): Observable<INavApp> {
    return this.apps;
  }
}
