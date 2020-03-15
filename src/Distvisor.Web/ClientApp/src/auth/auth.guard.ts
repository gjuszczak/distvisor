import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, UrlTree } from '@angular/router';
import { ApplicationPaths, QueryParameterNames } from './auth.constants';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private user: AuthService, private router: Router) {
  }

  canActivate(_next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> {
    return this.user.isAuthenticated().pipe(
      take(1),
      map(auth => {
        if (auth) {
          return true;
        }

        return this.router.createUrlTree(ApplicationPaths.LoginPathComponents, {
          queryParams: {
            [QueryParameterNames.ReturnUrl]: state.url
          }
        });
      }));
  }
}
