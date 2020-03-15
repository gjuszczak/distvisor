/* tslint:disable */
import { OAuthTokenIssuer } from './o-auth-token-issuer';
export interface OAuthToken {
  accessToken?: null | string;
  expiresIn?: number;
  issuer?: OAuthTokenIssuer;
  refreshToken?: null | string;
  scope?: null | string;
  tokenType?: null | string;
  utcIssueDate?: string;
}
