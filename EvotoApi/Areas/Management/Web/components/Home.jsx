import React from "react";
import {IndexLink} from "react-router";

class Home extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return(
            <div className="content-wrapper" style={{ height: "100%" }}>
                <section className="content-header" style={{ height: "100%" }}>
                    <h1>
                        evoto Management
                        <small>Manage votes and stuff</small>
                    </h1>
                    <ol className="breadcrumb">
                        <li>
                            <IndexLink to="/"><i className="fa fa-dashboard"/>Server Control</IndexLink>
                        </li>
                    </ol>
                </section>

                <section className="content">
                </section>
            </div>
        );
    }
}

export default Home