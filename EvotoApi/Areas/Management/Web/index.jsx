import React from 'react'
import ReactDOM from 'react-dom'
import {Router, Route, useRouterHistory, IndexRoute} from 'react-router'
import { createHistory } from 'history'
import PromisePolyfill from 'promise-polyfill'
import App from './App.jsx'
import Home from './components/Home.jsx'
import LoginContent from './components/LoginContent.jsx'
import NewVote from './components/vote/New.jsx'
import EditVote from './components/vote/Edit.jsx'
import NewUser from './components/user/New.jsx'
import ListUsers from './components/user/List.jsx'
import DetailUser from './components/user/Detail.jsx'

/* Polyfills */
if (!window.Promise) {
  window.Promise = PromisePolyfill
}

const history = useRouterHistory(createHistory)({
  basename: '/manage'
})

history.listen(() => {
  window.isDirty = false
})

$('#app').on('change keyup keydown', ':input', () => {
  window.isDirty = true
})

ReactDOM.render(
  <Router history={history}>
    <Route path='/login' component={LoginContent} />
    <Route path='/' component={App}>
      <IndexRoute component={Home} />
      <Route path='vote'>
        <Route path='new' component={NewVote} />
        <Route path=':id/edit' component={EditVote} />
      </Route>
      <Route path='user'>
        <Route path='new' component={NewUser} />
        <Route path='list' component={ListUsers} />
        <Route path=':id' component={DetailUser} />
      </Route>
    </Route>
  </Router>,
  document.getElementById('app'))
