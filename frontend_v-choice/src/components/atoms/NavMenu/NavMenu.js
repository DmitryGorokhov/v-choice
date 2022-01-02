import React, { Component } from 'react'
import { Link } from 'react-router-dom'
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import './NavMenu.css'

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor(props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
      userEmail: null
    };
  }

  toggleNavbar() {
    this.setState({
      collapsed: !this.state.collapsed
    });
  }

  componentDidMount() {
    this.checkUserAuth();
  }

  checkUserAuth() {
    fetch("https://localhost:5001/api/account/isAuthenticated", {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
    })
      .then(response => response.json())
      .then(result => {
        result.message === "guest"
          ? this.setState({ userEmail: null })
          : this.setState({ userEmail: result.message })
      });
  }

  handleClickLogout = () => {
    fetch("https://localhost:5001/api/account/logoff", {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
    })
      .then(response => response.json())
      .then(result => this.setState({ userEmail: null })
      );
  }

  render() {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/">v_choice</NavbarBrand>
            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
              {
                this.state.userEmail
                  ? <ul className="navbar-nav flex-grow" >
                    <NavItem>
                      <NavLink tag={Link} className="text-dark" to="/user">{this.state.userEmail}</NavLink>
                    </NavItem>
                    <NavItem>
                      <NavLink
                        tag={Link}
                        className="text-dark"
                        onClick={this.handleClickLogout}
                        to="/"
                      >
                        Выйти
                      </NavLink>
                    </NavItem>
                  </ul>
                  :
                  <ul className="navbar-nav flex-grow" >
                    <NavItem>
                      <NavLink tag={Link} className="text-dark" to="/sign-in">Вход</NavLink>
                    </NavItem>
                    <NavItem>
                      <NavLink tag={Link} className="text-dark" to="/sign-up">Регистрация</NavLink>
                    </NavItem>
                  </ul>
              }
            </Collapse>
          </Container>
        </Navbar>
      </header >
    );
  }
}
