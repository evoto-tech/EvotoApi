﻿import React from "react";
import {Link, IndexLink, browserHistory} from "react-router";

class Header extends React.Component {
    constructor(props) {
        super(props);
        this.onLogout = this.onLogout.bind(this);
    }

    contextTypes: {
        router: React.PropTypes.func.isRequired
    }

    propTypes: {
        loggedIn: React.PropTypes.bool.isRequired,
        username: React.PropTypes.string.isRequired
    }

    onLogout(e) {
        e.preventDefault();
        $.ajax({
            url: "/api/logout",
            dataType: "json",
            success: (resp) => {
                console.log(resp);
            }
        });
        // Wait for 1 second for logout callback to complete
        setTimeout(() => {
                browserHistory.push("/login");
            },
            1000);
    }

    render() {
        let loginMenu;
        if (this.props.loggedIn) {
            loginMenu =
            (<ul className="nav navbar-nav">
                 <li>
                     <Link to="/settings"><i className="fa fa-gears fa-fw"/>Settings</Link>
                 </li>
                 <li>
                     <a href="javascript:void(0)" onClick={this.onLogout}><i className="fa fa-lock fa-fw"/>Logout</
                     a>
                 </li>
             </ul>);
        }
        return(
            <header className="main-header">

                <IndexLink className="logo" to="/">
                    <span className="logo-lg"><b>evoto</b></span>
                </IndexLink>

                <nav className="navbar navbar-static-top" role="navigation">
                    <a href="#" className="sidebar-toggle" data-toggle="offcanvas" role="button">
                        <span className="sr-only">Toggle navigation</span>
                    </a>
                    <div className="navbar-custom-menu">
                        {loginMenu}
                    </div>
                </nav>
            </header>
        );
    }
}

export default Header