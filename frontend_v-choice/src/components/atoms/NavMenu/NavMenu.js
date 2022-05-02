import React, { useContext } from 'react'
import { Link } from 'react-router-dom'
import { Container, Navbar, NavbarBrand, NavItem, NavLink } from 'reactstrap'
import './NavMenu.css'

import UserContext from '../../../context'

export function NavMenu() {
  const { user, setUser } = useContext(UserContext);

  const handleLogout = () => {
    fetch("https://localhost:5001/api/account/logoff", {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
    })
      .then(_ => setUser({ userName: null, isAdmin: false }));
  }

  return (
    <header>
      <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
        <Container>
          <NavbarBrand tag={Link} to="/catalog">v_choice</NavbarBrand>
          {
            user.userName
              ? <ul className="navbar-nav flex-grow" >
                {
                  user.isAdmin
                    ? < NavItem >
                      <NavLink tag={Link} className="text-dark" to="/stat">cтатистика</NavLink>
                    </NavItem>
                    : null
                }
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/user">{user.userName}</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink
                    tag={Link}
                    className="text-dark"
                    onClick={handleLogout}
                    to="/catalog"
                  >
                    выйти
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
