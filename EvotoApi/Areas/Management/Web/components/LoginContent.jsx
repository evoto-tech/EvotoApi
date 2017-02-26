import React from 'react'
import {withRouter} from 'react-router'

class LoginContent extends React.Component {
  constructor (props) {
    super(props)
    this.loginUser = this.loginUser.bind(this)
  }

  componentDidMount () {
    console.log(this.props.router)
  }

  loginUser (e) {
    e.preventDefault()
    let loginModel = {
      client_id: "Management",
      grant_type: "password",
      username: this.refs.email.value,
      password: this.refs.password.value
    }

    $.ajax({
      type: 'POST',
      url: '/Token',
      dataType: 'json',
      data: loginModel,
      complete: (resp) => {
        console.log(resp)
        //this.props.router.push('/')
      }
    })
  }

  render () {
    return (
      <div className='container '>
        <div className='row'>
          <div className='absolute-center is-responsive'>
            <div className='col-centered col-md-4 col-mod-offset-2'>
              <div className='center-block'>
                <section className='content-header'>
                  <h1>
                                        evoto Manager
                                        <small>Login to manage evoto</small>
                  </h1>
                </section>

                <section className='content'>
                  <div className='row'>
                    <div className='login-box-body'>

                      <form onSubmit={this.loginUser}>
                        <div className='form-group has-feedback'>
                          <input type='text' ref='email' className='form-control' placeholder='Email' />
                          <span className='fa fa-envelope form-control-feedback' />
                        </div>
                        <div className='form-group has-feedback'>
                          <input type='password' ref='password' className='form-control' placeholder='Password' />
                          <span className='fa fa-lock form-control-feedback' />
                        </div>
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
                            <button type='submit' className='btn btn-primary btn-block btn-flat'>Sign In</button>
                          </div>
                        </div>
                      </form>

                      {/* <a href="#">I forgot my password</a><br /> */}
                    </div>
                  </div>
                </section>
              </div>
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default withRouter(LoginContent)
