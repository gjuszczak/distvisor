export const ApplicationName = 'Distvisor';

export const LocalStorageUserKey = `${ApplicationName}_Auth_User`;

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
  Login: `auth/${AuthActions.Login}`,
  LogOut: `auth/${AuthActions.Logout}`,
  LoggedOut: `auth/${AuthActions.LoggedOut}`,
  LoginPathComponents: [],
  LogOutPathComponents: [],
  LoggedOutPathComponents: [],
  ApiLogin: '',
  ApiLogout: '',
};

applicationPaths = {
  ...applicationPaths,
  LoginPathComponents: applicationPaths.Login.split('/'),
  LogOutPathComponents: applicationPaths.LogOut.split('/'),
  LoggedOutPathComponents: applicationPaths.LoggedOut.split('/'),
  ApiLogin: `api/${applicationPaths.Login}`,
  ApiLogout: `api/${applicationPaths.LogOut}`,
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
  readonly ApiLogout: string;
}

export const ApplicationPaths: ApplicationPathsType = applicationPaths;
