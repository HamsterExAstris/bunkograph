import { UnauthenticatedTemplate } from '@azure/msal-react'
import React, { Component } from 'react'
import { Navigate } from 'react-router-dom'
import { ApplicationPaths, QueryParameterNames } from './ApiAuthorizationConstants'

export default class AuthorizeRoute extends Component {
  render() {
    const { ready } = this.state;
    var link = document.createElement("a");
    link.href = this.props.path;
    const returnUrl = `${link.protocol}//${link.host}${link.pathname}${link.search}${link.hash}`;
    const redirectUrl = `${ApplicationPaths.Login}?${QueryParameterNames.ReturnUrl}=${encodeURIComponent(returnUrl)}`;
    if (!ready) {
      return <div></div>;
    } else {
      const { element } = this.props;
      return <>
        <AuthenticatedTemplate>
          { element }
        </AuthenticatedTemplate>
        <UnauthenticatedTemplate>
          <Navigate replace to={redirectUrl} />
        </UnauthenticatedTemplate>
      </>
    }
  }
}
