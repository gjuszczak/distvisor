import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, UrlTree } from '@angular/router';
import { ApplicationPaths, QueryParameterNames } from './authorization.constants';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeGuard implements CanActivate {
  constructor(private user: UserService, private router: Router) {
  }

  canActivate(_next: ActivatedRouteSnapshot, state: RouterStateSnapshot) : boolean | UrlTree {
      if(this.user.isAuthenticated())
        return true;

      return this.router.createUrlTree(ApplicationPaths.LoginPathComponents, {
        queryParams: {
          [QueryParameterNames.ReturnUrl]: state.url
        }
      });
  }
}
