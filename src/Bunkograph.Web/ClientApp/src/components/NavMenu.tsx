import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { Button, Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { loginRequest } from "../authConfig";
import './NavMenu.css';
import { SignInButton } from "./SignInButton";

export interface INavMenuState {
  collapsed: boolean
}

function ProfileContent() {
  const { instance, accounts } = useMsal();
  const [accessToken, setAccessToken] = useState<string | undefined>();
 
  const name = accounts[0] && accounts[0].name;

  function RequestAccessToken() {
    const request = {
      ...loginRequest,
      account: accounts[0]
    };

    // Silently acquires an access token which is then attached to a request for Microsoft Graph data
    instance.acquireTokenSilent(request).then((response) => {
      setAccessToken(response.accessToken);
    }).catch((e) => {
      instance.acquireTokenPopup(request).then((response) => {
        setAccessToken(response.accessToken);
      });
    });
  }

  return (
    <>
      <h5 className="card-title">Welcome {name}</h5>
      {accessToken ?
        <p>Access Token Acquired!</p>
        :
        <Button variant="secondary" onClick={RequestAccessToken}>Request Access Token</Button>
      }
    </>
  );
};

export const NavMenu: React.FC = () => {
  const [collapsed, setCollapsed] = useState<boolean>(true);

  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  }

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
        <NavbarBrand tag={Link} to="/">Bunkograph.Web</NavbarBrand>
        <NavbarToggler onClick={toggleNavbar} className="mr-2" />
        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
          <ul className="navbar-nav flex-grow">
            <NavItem>
              <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
            </NavItem>
            <NavItem>
              <NavLink tag={Link} className="text-dark" to="/series">Series</NavLink>
            </NavItem>
            <AuthenticatedTemplate>
              <ProfileContent />
            </AuthenticatedTemplate>
            <UnauthenticatedTemplate>
              <SignInButton />
            </UnauthenticatedTemplate>
          </ul>
        </Collapse>
      </Navbar>
    </header>
  );
}
