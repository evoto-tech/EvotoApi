import React from "react"
import ReactDOM from "react-dom"
import {Router, Route, useRouterHistory, IndexRoute} from "react-router"
import { createHistory } from "history"
import App from "./App.jsx"
import Home from "./components/Home.jsx"
import LoginContent from './components/LoginContent.jsx'

const history = useRouterHistory(createHistory)({
    basename: '/manage'
})

ReactDOM.render(
    <Router history={history}>
        <Route path="/login" component={LoginContent}/>
        <Route path="/" component={App}>
            <IndexRoute component={Home}/>
        </Route>
    </Router>,
    document.getElementById("app"));