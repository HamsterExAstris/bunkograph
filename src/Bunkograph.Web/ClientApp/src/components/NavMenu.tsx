import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import React, { useState } from 'react';
import { Button, Container, Nav, Navbar, NavItem } from 'react-bootstrap';
import { Link } from 'react-router-dom';
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
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" bg="light">
        <Container>
          <Navbar.Brand as={Link} to="/">Bunkograph.Web</Navbar.Brand>
          <Navbar.Toggle onClick={toggleNavbar} className="mr-2" />
          <Navbar.Collapse className="d-sm-inline-flex flex-sm-row-reverse" role="navigation">
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <Nav.Link as={Link} className="text-dark" to="/">Home</Nav.Link>
              </NavItem>
              <NavItem>
                <Nav.Link as={Link} className="text-dark" to="/series">Series</Nav.Link>
              </NavItem>
              <AuthenticatedTemplate>
                <ProfileContent />
              </AuthenticatedTemplate>
              <UnauthenticatedTemplate>
                <SignInButton />
              </UnauthenticatedTemplate>
            </ul>
            </Navbar.Collapse>
          </Container>
      </Navbar>
    </header>
  );
}
