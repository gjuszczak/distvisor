import { Subject, Observable } from "rxjs";

export class NavigationService {
  private navBrand = new Subject<string>();
  private logoutVisible = new Subject<boolean>();

  getNavBrand() : Observable<string> {
    return this.navBrand;
  }

  setNavBrand(brand: string) {
    this.navBrand.next(brand);
  }

  getLogoutVisible(): Observable<boolean> {
    return this.logoutVisible;
  }

  setLogoutVisible(isVisible: boolean) {
    this.logoutVisible.next(isVisible);
  }
}
