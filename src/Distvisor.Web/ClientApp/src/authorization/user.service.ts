import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { LocalStorageUserKey } from './authorization.constants';

export interface IUser {
  username: string;
  sessionId: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {

  public isAuthenticated(): boolean {
    return !!this.getUser();
  }

  public getUser(): IUser | null {
    var userJson = localStorage.getItem(LocalStorageUserKey);
    var user = userJson && JSON.parse(userJson);
    return user;
  }

  public setUser(user: IUser) {
    var userJson = JSON.stringify(user);
    localStorage.setItem(LocalStorageUserKey, userJson);
  }

  public clearUser() {
    localStorage.removeItem(LocalStorageUserKey);
  }
}
