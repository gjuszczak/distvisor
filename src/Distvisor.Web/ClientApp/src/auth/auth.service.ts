import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';
import { LocalStorageUserKey } from './auth.constants';
import { AuthUser } from '../api/models';
import { AuthService as AuthClient } from '../api/services';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private userSubject: BehaviorSubject<AuthUser | null>;
  private user: AuthUser | null;

  constructor(private authClient: AuthClient) {
    this.user = this.getUserFromLocalStorage();
    this.userSubject = new BehaviorSubject(this.user);
  }

  public isAuthenticated(): Observable<boolean> {
    return this.userSubject.pipe(map(u => !!u));
  }

  public getUser(): AuthUser | null {
    return this.user;
  }

  public login(username: string, password: string): Observable<AuthUser> {
    return this.authClient.apiAuthLoginPost$Json$Json({ body: { username, password } })
      .pipe(tap(user => this.setUser(user), _ => this.clearUser()));
  }

  public logout(): Observable<void> {
    return this.authClient.apiAuthLogoutPost()
      .pipe(tap(_ => this.clearUser(), _ => this.clearUser()));
  }

  public refreshToken(): Observable<string> {
    const refreshToken = this.user.refreshToken;
    return this.authClient.apiAuthRefreshtokenPost$Json$Json({ body: { refreshToken } })
      .pipe(
        tap(user => this.setUser(user), _ => this.clearUser()),
        map(user => user.accessToken)
      );
  }

  private setUser(user: AuthUser) {
    var userJson = JSON.stringify(user);
    localStorage.setItem(LocalStorageUserKey, userJson);
    this.user = user;
    this.userSubject.next(user);
  }

  private clearUser() {
    localStorage.removeItem(LocalStorageUserKey);
    this.user = null;
    this.userSubject.next(null);
  }

  private getUserFromLocalStorage(): AuthUser | null {
    var userJson = localStorage.getItem(LocalStorageUserKey);
    var user = userJson && JSON.parse(userJson);
    return user;
  }
}
