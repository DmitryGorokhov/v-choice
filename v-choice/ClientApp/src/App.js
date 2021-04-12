import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import Films from './components/Films';
import { SignIn } from "./components/SignIn";
import { SignUp } from "./components/SignUp";

import './custom.css'
import 'fontsource-roboto';
import FilmPage from './components/FilmPage';

export default class App extends Component {
  static displayName = App.name;

  render() {

    return (
      <Layout>
        <Route exact path='/' component={Films} />
        <Route path='/sign-in' component={SignIn} />
        <Route path='/sign-up' component={SignUp} />
        <Route path='/film' component={FilmPage} />
      </Layout>
    );
  }
}
