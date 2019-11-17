import { Subject, Observable } from "rxjs";

export class NavigationService {
    private navBrand = new Subject<string>();

    getNavBrand() : Observable<string> {
        return this.navBrand;
    }

    setNavBrand(brand: string) {
        this.navBrand.next(brand);
    }
}