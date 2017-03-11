import React from 'react'
import { withRouter } from 'react-router'

class Login extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      email: '',
      password: '',
      ready: false,
      error: '',
      errorMessages: []
    }
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

  login (e) {
    e.preventDefault()
    let user = {
          email: this.state.email,
          password: this.state.password
        }

    let handleError = (err, errorMessages) => {
      errorMessages = errorMessages || []
      console.error('Login error', err, errorMessages)
      this.setState({
        error: 'Those details seem to be wrong! Please try again.',
        errorMessages
      })
    }

    fetch('/mana/auth/login',
      { method: 'POST',
        credentials: 'same-origin',
        body: JSON.stringify(user),
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json; charset=utf-8'
        }
      })
      .then((response) => {
        if (response.ok && response.status === 200) {
          return this.props.router.push('/')
        } else if (response.status === 401) {
          return handleError(response)
        } else if (response.status === 400) {
          return response.json()
        } else {
          return handleError(response)
        }
      }, handleError)
      .then((data) => {
        let messages = data.ModelState
          ? Object.keys(data.ModelState).reduce((o, k) => data.ModelState[k], [])
          : undefined
        return handleError(data, messages)
      }, handleError)
      .catch(handleError)
  }

  render () {
    let errorMessages = this.state.errorMessages.length > 0 ? (
      <ul>
        {this.state.errorMessages.map((message, i) => (
          <li key={i}>{message}</li>
        ))}
      </ul>
    ) : ''
    return (
      <div>
        <form onSubmit={this.login.bind(this)}>
          { this.state.error === '' ? '' : (
              <div className='callout callout-danger'>
                <h4>Error!</h4>
                <p>{this.state.error}</p>
                {errorMessages}
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
        </form>
      </div>
    )
  }
}

export default withRouter(Login)
