import React, { Component } from 'react'
import { Link } from 'react-router-dom'
import { Container, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap';
import './NavMenu.css'

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor(props) {
    super(props);

    this.state = {
      userEmail: null
    };
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
        result.userName === "guest"
          ? this.setState({ userEmail: null })
          : this.setState({ userEmail: result.userName });

        if (this.props.onLoadUser && this.props.onLoadUser !== undefined) {
          this.props.onLoadUser(result.userName);
        }
      })
      .catch(_ => this.setState({ userEmail: null }));
  }

  handleClickLogout = () => {
    fetch("https://localhost:5001/api/account/logoff", {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
    })
      .then(_ => this.setState({ userEmail: null })
      );

    if (this.props.onLogout && this.props.onLogout !== undefined) {
      this.props.onLogout();
    }
  }

  render() {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/catalog">v_choice</NavbarBrand>
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
                      to="/catalog"
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
          </Container>
        </Navbar>
      </header >
    );
  }
}
