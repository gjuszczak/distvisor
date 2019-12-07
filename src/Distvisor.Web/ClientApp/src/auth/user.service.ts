import { Injectable } from '@angular/core';
import { LocalStorageUserKey } from './auth.constants';
import { Observable, BehaviorSubject, concat, of } from 'rxjs';
import { take, filter, tap, map } from 'rxjs/operators';

export interface IUser {
  username: string;
  sessionId: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private userSubject: BehaviorSubject<IUser | null> = new BehaviorSubject(null);

  constructor() {
    const user = this.getUserFromLocalStorage();
    this.userSubject = new BehaviorSubject(user);    
  }

  public isAuthenticated(): Observable<boolean> {
    return this.getUser().pipe(map(u => !!u));
  }

  public getUser(): Observable<IUser | null> {
    return this.userSubject;
  }

  public setUser(user: IUser) {
    var userJson = JSON.stringify(user);
    localStorage.setItem(LocalStorageUserKey, userJson);
    this.userSubject.next(user);
  }

  public clearUser() {
    localStorage.removeItem(LocalStorageUserKey);
    this.userSubject.next(null);
  }

  private getUserFromLocalStorage(): IUser | null {
    var userJson = localStorage.getItem(LocalStorageUserKey);
    var user = userJson && JSON.parse(userJson);
    return user;
  }
}
