import React from 'react'
import { withRouter } from 'react-router'

class Register extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      firstName: '',
      lastName: '',
      email: '',
      password: '',
      confirmPassword: '',
      matchingPassword: true,
      ready: false,
      error: '',
      errorMessages: []
    }
  }

  checkReady () {
    let ready = this.state.matchingPassword
        && this.state.firstName !== ''
        && this.state.lastName !== ''
        && this.state.email !== ''
        && this.state.password !== ''
    this.setState({ ready })
  }

  onFieldChange (e, cb) {
    let update = {}
    update[e.target.dataset['field']] = e.target.value
    this.setState(update, cb || this.checkReady)
  }

  checkPassword (e) {
    this.onFieldChange(e, () => {
      let matchingPassword = (this.state.password === this.state.confirmPassword)
      this.setState({ matchingPassword }, this.checkReady)
    })
  }

  register (e) {
    e.preventDefault()
    let user = {
      FirstName: this.state.firstName,
      LastName: this.state.lastName,
      Email: this.state.email,
      Password: this.state.password,
      ConfirmPassword: this.state.confirmPassword
    }

    let handleError = (err, errorMessages) => {
      errorMessages = errorMessages || []
      console.error('Login error', err, errorMessages)
      this.setState({
        error: 'Those details seem to be wrong! Please try again.',
        errorMessages
      })
    }

    fetch('/api/Account/Register',
      { method: 'POST',
        body: JSON.stringify(user),
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      })
      .then((response) => {
        if (response.ok && response.status === 200) {
          return this.props.router.push('/login')
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
        <form onSubmit={this.register.bind(this)}>
          { this.state.error === '' ? '' : (
              <div className='callout callout-danger'>
                <h4>Error!</h4>
                <p>{this.state.error}</p>
                {errorMessages}
              </div>
            )
          }
          <div className='input-group' style={{ width: '100%' }}>
            <span className='fa fa-user input-group-addon' style={{ width: '40px' }}/>
            <input
              type='text'
              ref='firstName'
              data-field='firstName'
              className='form-control'
              placeholder='First Name'
              value={this.state.firstName}
              onChange={this.onFieldChange.bind(this)}
              />
          </div>
          <br/>
          <div className='input-group' style={{ width: '100%' }}>
            <span className='fa fa-user input-group-addon' style={{ width: '40px' }}/>
            <input
              type='text'
              ref='lastName'
              data-field='lastName'
              className='form-control'
              placeholder='Last Name'
              value={this.state.lastName}
              onChange={this.onFieldChange.bind(this)}
              />
          </div>
          <br/>
          <div className='input-group' style={{ width: '100%' }}>
            <span className='fa fa-envelope input-group-addon' style={{ width: '40px' }}/>
            <input
              type='text'
              ref='email'
              data-field='email'
              className='form-control'
              placeholder='Email'
              value={this.state.email}
              onChange={this.onFieldChange.bind(this)}
              />
          </div>
          <br/>
          <div className='input-group'style={{ width: '100%' }}>
            <span className='fa fa-lock input-group-addon' style={{ width: '40px' }} />
            <input
              type='password'
              ref='password'
              data-field='password'
              className='form-control'
              placeholder='Password'
              value={this.state.password}
              onChange={this.checkPassword.bind(this)}
              />
          </div>
          <br/>
          <div className={`input-group ${this.state.matchingPassword ? '' : 'has-error' }`} style={{ width: '100%' }}>
            <span className='fa fa-lock input-group-addon' style={{ width: '40px' }} />
            <input
              type='password'
              ref='confirmPassword'
              data-field='confirmPassword'
              className='form-control'
              placeholder='Confirm password'
              value={this.state.confirmPassword}
              onChange={this.checkPassword.bind(this)}
              />
          </div>
          {this.state.matchingPassword ? '' : (
            <div className='input-group has-error'style={{ width: '100%' }}>
              <span className='has-error help-block'>Passwords do not match</span>
            </div>
          )}
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
              <button type='submit' disabled={!this.state.ready} className='btn btn-success btn-block btn-flat' onClick={this.register.bind(this)}>Register</button>
            </div>
          </div>
          {/* <a href="#">I forgot my password</a><br /> */}
        </form>
      </div>
    )
  }
}

export default withRouter(Register)
