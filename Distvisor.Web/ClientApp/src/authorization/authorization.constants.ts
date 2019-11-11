export const ApplicationName = 'Distvisor';

export const LocalStorageKeys = {
  User: `${ApplicationName}_Auth_User`,
  Session: `${ApplicationName}_Auth_Session`
}

export const ReturnUrlType = 'returnUrl';

export const QueryParameterNames = {
  ReturnUrl: ReturnUrlType,
  Message: 'message'
};

export const AuthActions = {
  Login: 'login',
  Logout: 'logout',
  LoggedOut: 'logged-out'
};

let applicationPaths: ApplicationPathsType = {
  DefaultLoginRedirectPath: '/',
  Login: `authentication/${AuthActions.Login}`,
  LogOut: `authentication/${AuthActions.Logout}`,
  LoggedOut: `authentication/${AuthActions.LoggedOut}`,
  LoginPathComponents: [],
  LogOutPathComponents: [],
  LoggedOutPathComponents: [],
  ApiLogin: ``,
};

applicationPaths = {
  ...applicationPaths,
  LoginPathComponents: applicationPaths.Login.split('/'),
  LogOutPathComponents: applicationPaths.LogOut.split('/'),
  LoggedOutPathComponents: applicationPaths.LoggedOut.split('/'),
  ApiLogin: `api/${applicationPaths.Login}`,
};

interface ApplicationPathsType {
  readonly DefaultLoginRedirectPath: string;
  readonly Login: string;
  readonly LogOut: string;
  readonly LoggedOut: string;
  readonly LoginPathComponents: string [];
  readonly LogOutPathComponents: string [];
  readonly LoggedOutPathComponents: string [];
  readonly ApiLogin: string;
}

export const ApplicationPaths: ApplicationPathsType = applicationPaths;
