/* tslint:disable */
import { OAuthToken } from './o-auth-token';
export interface AuthResult {
  isAuthenticated?: boolean;
  message?: null | string;
  token?: null | OAuthToken;
  userId?: string;
  username?: null | string;
}
