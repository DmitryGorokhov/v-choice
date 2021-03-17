import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import Films from './components/Films';

import './custom.css'
import 'fontsource-roboto';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route path='/' component={Films} />
      </Layout>
    );
  }
}
