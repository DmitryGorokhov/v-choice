import React, { Component } from 'react'
import { Route } from 'react-router'
import './custom.css'
import 'fontsource-roboto'

import Films from './components/pages/Films/Films'
import { SignIn } from './components/pages/SignIn/SignIn'
import { SignUp } from './components/pages/SignUp/SignUp'
import FilmPage from './components/pages/FilmPage/FilmPage'
import { Layout } from './components/atoms/Layout/Layout'
import UserPage from './components/pages/UserPage/UserPage'

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path='/catalog' component={Films} />
        <Route path='/catalog/:page/:count/:genre/:type/:order/:onlyc/:norate' component={Films} />
        <Route path='/sign-in' component={SignIn} />
        <Route path='/sign-up' component={SignUp} />
        <Route path='/film/:slug' component={FilmPage} />
        <Route path='/user' component={UserPage} />
      </Layout>
    );
  }
}
