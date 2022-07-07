import { useIsAuthenticated } from '@azure/msal-react';
import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { Collapse, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap';
import './NavMenu.css';
import { SignInButton } from "./SignInButton";

export interface INavMenuState {
  collapsed: boolean
}

export const NavMenu: React.FC = () => {
//export class NavMenu extends Component<undefined, INavMenuState> {
  // static displayName = NavMenu.name;
  const isAuthenticated = useIsAuthenticated();

  const [collapsed, setCollapsed] = useState<boolean>(true);

  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  }

  /*
  constructor(props: undefined) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true
    };
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }
  */

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
        <NavbarBrand tag={Link} to="/">Bunkograph.Web</NavbarBrand>
        { // <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
        }
        <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
          <ul className="navbar-nav flex-grow">
            <NavItem>
              <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
            </NavItem>
            <NavItem>
              <NavLink tag={Link} className="text-dark" to="/series">Series</NavLink>
            </NavItem>
            {isAuthenticated ? <span>Signed In</span> : <SignInButton />}
          </ul>
        </Collapse>
      </Navbar>
    </header>
  );
}
