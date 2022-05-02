import React, { useEffect, useState } from 'react'
import { Route } from 'react-router'
import './custom.css'
import 'fontsource-roboto'

import Films from './components/pages/Films/Films'
import { SignIn } from './components/pages/SignIn/SignIn'
import { SignUp } from './components/pages/SignUp/SignUp'
import FilmPage from './components/pages/FilmPage/FilmPage'
import { Layout } from './components/atoms/Layout/Layout'
import UserPage from './components/pages/UserPage/UserPage'
import Statistic from './components/pages/Statistic/Statistic'
import UserContext from './context'
import { NavMenu } from './components/atoms/NavMenu/NavMenu'

export default function App() {
  const [user, setUser] = useState({
    userName: null,
    isAdmin: false,
  });

  useEffect(() => {
    fetch("https://localhost:5001/api/account/isAuthenticated", {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
    })
      .then(response => response.json())
      .then(result => {
        result.userName === "guest"
          ? setUser({ userName: null, isAdmin: false })
          : setUser({ userName: result.user.userName, isAdmin: result.user.isAdmin });
      })
      .catch(_ => setUser({ userName: null, isAdmin: false }));
  }, [])

  return (
    <UserContext.Provider value={{ user, setUser }}>
      <NavMenu />
      <Layout>
        <Route exact path='/catalog' component={Films} />
        <Route path='/catalog/:slug' component={Films} />
        <Route path='/sign-in' component={SignIn} />
        <Route path='/sign-up' component={SignUp} />
        <Route path='/film/:slug' component={FilmPage} />
        <Route path='/user' component={UserPage} />
        <Route path='/stat' component={Statistic} />
      </Layout>
    </UserContext.Provider>
  );
}
