import { Injectable } from '@angular/core';
import { LocalStorageUserKey } from './auth.constants';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthResult } from 'src/api/models';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private userSubject: BehaviorSubject<AuthResult | null> = new BehaviorSubject(null);

  constructor() {
    const user = this.getUserFromLocalStorage();
    this.userSubject = new BehaviorSubject(user);    
  }

  public isAuthenticated(): Observable<boolean> {
    return this.getUser().pipe(map(u => !!u));
  }

  public getUser(): Observable<AuthResult | null> {
    return this.userSubject;
  }

  public setUser(user: AuthResult) {
    var userJson = JSON.stringify(user);
    localStorage.setItem(LocalStorageUserKey, userJson);
    this.userSubject.next(user);
  }

  public clearUser() {
    localStorage.removeItem(LocalStorageUserKey);
    this.userSubject.next(null);
  }

  private getUserFromLocalStorage(): AuthResult | null {
    var userJson = localStorage.getItem(LocalStorageUserKey);
    var user = userJson && JSON.parse(userJson);
    return user;
  }
}
