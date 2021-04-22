import React from 'react';
import { Router, Route, Switch, Redirect, } from "react-router-dom";
import history from './history/History';
import { NotFound } from './pages/NotFound';
import { LoginForm } from './pages/LoginForm';
import { Home } from './pages/Home';
import PrivateRoute from './components/PrivateRoute';

function App() {
  return (
    <div>
      <Router history={history} >
        <Switch >
          <Route path="/" exact component={LoginForm} />
          <Route path="/login" exact component={LoginForm} />
          {/* home下有二级路由的话，这里不能用exact */}
          <PrivateRoute path="/home" component={Home} />
          <Route path="/notFound" component={NotFound} />
          <Redirect to="/notFound" />
        </Switch>
      </Router>
    </div>
  );
}

export default App;
