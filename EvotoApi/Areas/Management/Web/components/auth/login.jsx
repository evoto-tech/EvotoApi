import React from 'react'
import { withRouter } from 'react-router'
import Session from '../../lib/session'

class Login extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      email: '',
      password: '',
      ready: false,
      error: ''
    }
    this.session = new Session()
  }

  checkReady () {
    let ready = (this.state.email !== '' && this.state.password !== '')
    this.setState({ ready })
  }

  onFieldChange (e, cb) {
    let update = {}
    update[e.target.dataset['field']] = e.target.value
    this.setState(update, cb || this.checkReady)
  }

  login () {
    let form = [
        `username=${encodeURIComponent(this.state.email)}`,
        `password=${encodeURIComponent(this.state.password)}`,
        `grant_type=password`
      ].join('&')

    fetch('/Token',
      { method: 'POST',
        body: form,
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/x-www-form-urlencoded; charset=utf-8'
        }
      })
      .then((data) => {
        return data.json()
      }, console.error)
      .then((json) => {
        if (json.error) {
          this.setState({ error: 'Those details seem to be wrong! Please try again.' })
        } else {
          this.session.storeTokens(json['access_token'], json['refresh_token'], json['.expires'])
          this.props.router.push('/')
        }
      })
      .catch((err) => {
        this.setState({ error: err.Message })
      })
  }

  render () {
    return (
      <div>
        { this.state.error === '' ? '' : (
            <div className='callout callout-danger'>
              <h4>Error!</h4>
              <p>{this.state.error}</p>
            </div>
          )
        }
        <div className='input-group' style={{ width: '100%' }}>
          <span className='fa fa-envelope input-group-addon' style={{ width: '40px' }}/>
          <input
            type='text'
            data-field='email'
            ref='email'
            className='form-control'
            placeholder='Email'
            onChange={this.onFieldChange.bind(this)}
            />
        </div>
        <br/>
        <div className='input-group'style={{ width: '100%' }}>
          <span className='fa fa-lock input-group-addon' style={{ width: '40px' }} />
          <input
            type='password'
            data-field='password'
            ref='password'
            className='form-control'
            placeholder='Password'
            onChange={this.onFieldChange.bind(this)}
            />
        </div>
        <br/>
        {/* <div className="row">
              <div className="col-xs-8">
                  <div className="checkbox">
                      <label className="">
                          <div className="" aria-checked="false" aria-disabled="false" style={{position: "relative"}}>
                          <input type="checkbox"/>
                          </div> Remember Me
                      </label>
                  </div>
              </div>
            </div>
        */}
        <div className='row'>
          <div className='col-xs-4'>
            <button
              type='submit'
              disabled={!this.state.ready}
              className='btn btn-success btn-block btn-flat'
              onClick={this.login.bind(this)}
            >
              Log In
            </button>
          </div>
        </div>
        {/* <a href="#">I forgot my password</a><br /> */}
      </div>
    )
  }
}

export default withRouter(Login)
