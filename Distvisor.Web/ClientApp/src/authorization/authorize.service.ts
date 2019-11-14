import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BehaviorSubject, concat, of, Observable } from 'rxjs';
import { filter, map, take, tap } from 'rxjs/operators';
import { LocalStorageUserKey, ApplicationPaths } from './authorization.constants';

export interface IAuthenticationResult {
  status: AuthenticationResultStatus;
  message: string;
}

export enum AuthenticationResultStatus {
  Success,
  Fail
}

export interface IUser {
  username: string;
  sessionId: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  public isAuthenticated(): Observable<boolean> {
    return this.getUser()
      .pipe(map(u => !!u));
  }

  public getUser(): Observable<IUser | null> {
    return this.getUserFromStorage();
  }

  public signIn(username: string, password: string): Observable<IAuthenticationResult> {
    return this.http.post(
      this.baseUrl + ApplicationPaths.ApiLogin,
      { username, password },
      { observe: 'response' })
      .pipe(
        tap(resp => this.storeUser(<IUser>resp.body)),
        map(_ => this.success(), (err: HttpResponse<Object>) => this.fail(err.body.toString())
      ));
  }

  public signOut() {
    localStorage.setItem(LocalStorageUserKey, null);
  }

  private fail(message: string): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Fail, message };
  }

  private success(): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Success, message: "" };
  }

  private getUserFromStorage(): Observable<IUser | null> {
    return of(localStorage.getItem(LocalStorageUserKey)).pipe(
      map(u => u && JSON.parse(u)));
  }

  private storeUser(user: IUser) {
    localStorage.setItem(LocalStorageUserKey, JSON.stringify(user));
  }
}
