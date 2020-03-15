import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { mergeMap, catchError, take, switchMap, finalize, filter } from 'rxjs/operators';
import { ApplicationPaths } from './auth.constants';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class AuthInterceptor implements HttpInterceptor {

  isRefreshingToken: boolean = false;
  tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);

  constructor(
    private authService: AuthService,
    private router: Router) { }

  addToken(req: HttpRequest<any>, token: string): HttpRequest<any> {
    return req.clone({ setHeaders: { Authorization: 'Bearer ' + token } });
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const user = this.authService.getUser();
    if (!!user && this.isSameOriginUrl(req)) {
      return next.handle(this.addToken(req, user.accessToken))
        .pipe(catchError((error: HttpErrorResponse) => {
          if (error.status === 401) {
            return this.handle401Error(req, next);
          }
          return throwError(error);
        }));
    }
    return next.handle(req);
  }

  handle401Error(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshingToken) {
      this.isRefreshingToken = true;
      this.tokenSubject.next(null);

      return this.authService.refreshToken()
        .pipe(
          switchMap((newToken: string) => {
            if (newToken) {
              this.tokenSubject.next(newToken);
              return next.handle(this.addToken(req, newToken));
            }
            return this.logoutUser();
          }),
          catchError(_ => {
            return this.logoutUser();
          }),
          finalize(() => {
            this.isRefreshingToken = false;
          })
        );
    } else {
      return this.tokenSubject
        .pipe(
          filter(token => token != null),
          take(1),
          switchMap(token => {
            return next.handle(this.addToken(req, token));
          })
        );
    }
  }

  logoutUser() {
    this.router.navigate(ApplicationPaths.LogOutPathComponents);
    return throwError("");
  }

  private isSameOriginUrl(req: any) {
    // It's an absolute url with the same origin.
    if (req.url.startsWith(`${window.location.origin}/`)) {
      return true;
    }

    // It's a protocol relative url with the same origin.
    // For example: //www.example.com/api/Products
    if (req.url.startsWith(`//${window.location.host}/`)) {
      return true;
    }

    // It's a relative url like /api/Products
    if (/^\/[^\/].*/.test(req.url)) {
      return true;
    }

    // It's an absolute or protocol relative url that
    // doesn't have the same origin.
    return false;
  }
}
