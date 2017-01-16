import React from 'react'
import { withRouter } from 'react-router'
import Header from './components/Header.jsx'
import Sidebar from './components/Sidebar.jsx'
import Footer from './components/Footer.jsx'

class App extends React.Component {
  constructor (props) {
    super(props)
    this.checkLogin = this.checkLogin.bind(this)
    this.state = {
      loggedIn: true,
      username: 'Alan'
    }
  }

  contextTypes: {
    router: React.PropTypes.func.isRequired
  }

  componentDidMount () {
    this.checkLogin()
  }

  checkLogin () {
    $.ajax({
      url: '/api/user/status',
      dataType: 'json',
      success: (data) => {
        if (data.success === true) {
          this.setState({
            loggedIn: true,
            username: data.data.Username
          })
        } else {
          this.props.router.push('/manage/login')
        }
      }
    })
  }

  render () {
    // render main application,
    // if logged in show application
    // if not logged in show Not logged in message
    let resp
    if (this.state.loggedIn) {
      resp = (
        <div>
          <Header
            username={this.state.username}
            loggedIn={this.state.loggedIn}
            messages={this.state.messages} />

          <Sidebar username={this.state.username} />

          { // Render react-router components and pass in props
          React.cloneElement(this.props.children,
            { username: this.state.username })
          }

          <Footer />
        </div>)
    } else {
      resp = (<div><p>Not Logged in</p></div>)
    }

    return (
      <div className='wrapper'>
        {resp}
      </div>
    )
  }
}

export default withRouter(App)
