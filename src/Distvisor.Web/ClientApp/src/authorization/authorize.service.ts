import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApplicationPaths } from './authorization.constants';
import { IUser } from './user.service';

export interface IAuthenticationResult {
  success: boolean;
  user: IUser | null;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  public login(username: string, password: string): Observable<IAuthenticationResult> {
    return this.http.post(
      this.baseUrl + ApplicationPaths.ApiLogin,
      { username, password },
      { observe: 'response' })
      .pipe(map(
        resp => this.success(resp), 
        (err: HttpResponse<Object>) => this.fail(err)));
  }

  public logout(): Observable<Object> {
    return this.http.post(this.baseUrl + ApplicationPaths.ApiLogout, null);
  }

  private fail(response: HttpResponse<Object>): IAuthenticationResult {
    return {
      success: false,
      user: null,
      message: response.body.toString()
    };
  }

  private success(response: HttpResponse<Object>): IAuthenticationResult {
    return {
      success: true,
      user: <IUser>response.body,
      message: "ok"
    };
  }
}
