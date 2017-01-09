import React from "react"
import ReactDOM from "react-dom"
import {Router, Route, useRouterHistory, IndexRoute, createHashHistory} from "react-router"
import App from "./App.jsx"
import Home from "./components/Home.jsx"
import LoginContent from './components/LoginContent.jsx'


const history = useRouterHistory(createHashHistory)({
    basename: '/management'
})

ReactDOM.render(
    <Router history={history}>
        <Route path="/login" component={LoginContent}/>
        <Route path="/" component={App}>
            <IndexRoute component={Home}/>
        </Route>
    </Router>,
    document.getElementById("app"));