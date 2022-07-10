export const QueryParameterNames = {
  ReturnUrl: 'returnUrl'
};

export const LoginActions = {
  Login: 'login',
};

const prefix = '/authentication';

export const ApplicationPaths = {
  Login: `${prefix}/${LoginActions.Login}`,
};
