import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, concat, of, Observable } from 'rxjs';
import { filter, map, take, tap } from 'rxjs/operators';
import { LocalStorageKeys, ApplicationPaths } from './authorization.constants';

export interface IAuthenticationResult {
  status: AuthenticationResultStatus;
  message: string;
}

export enum AuthenticationResultStatus {
  Success,
  Fail
}

export interface IUser {
  name: string;
  accessToken: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {
  private userSubject: BehaviorSubject<IUser | null> = new BehaviorSubject(null);

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  public isAuthenticated(): Observable<boolean> {
    return this.getUser().pipe(map(u => !!u));
  }

  public getUser(): Observable<IUser | null> {
    return concat(
      this.userSubject.pipe(take(1), filter(u => !!u)),
      this.getUserFromStorage().pipe(filter(u => !!u), tap(u => this.userSubject.next(u))),
      this.userSubject.asObservable());
  }

  public signIn(username: string, password: string): Observable<IAuthenticationResult> {

    return this.http.post(
      this.baseUrl + ApplicationPaths.ApiLogin,
      { username, password },
      { observe: 'response' })
      .pipe(map(
        _ => this.success(),
        (error) => this.fail(error.body.toString())
      ));
  }

  public signOut(): Observable<IAuthenticationResult> {

    return of(this.fail("Dupa"));
  }

  private fail(message: string): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Fail, message };
  }

  private success(): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Success, message: "" };
  }

  private getUserFromStorage(): Observable<IUser> {
    return of(localStorage.getItem(LocalStorageKeys.User)).pipe(
      map(u => u && { name: u, accessToken: "" }));
  }
}
