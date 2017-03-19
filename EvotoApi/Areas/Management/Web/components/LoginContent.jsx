import React from 'react'
import Login from './auth/login.jsx'
import Register from './auth/register.jsx'

class LoginContent extends React.Component {
  render () {
    return (
      <div className='content'>
        <div className='container'>
          <div className='row'>
            <div className='absolute-center is-responsive'>
              <div className='col-centered col-md-4 col-mod-offset-2'>
                <div className='center-block'>
                  <div className='box box-success'>
                    <div className='box-header with-border'>
                      <h3 className='box-title'>evoto Management</h3>
                      <br />
                      <small>Access the evoto management portal.</small>
                    </div>
                    <div className='login-box-body'>
                      <div className='nav-tabs-custom tab-success'>
                        <ul className='nav nav-tabs'>
                          <li className='active'>
                            <a href='#tab_1' data-toggle='tab' aria-expanded='true'>Login</a>
                          </li>
                          <li>
                            <a href='#tab_2' data-toggle='tab' aria-expanded='false'>Register</a>
                          </li>
                        </ul>
                        <div className='tab-content'>
                          <div className='tab-pane active' id='tab_1'>
                            <Login />
                          </div>
                          <div className='tab-pane' id='tab_2'>
                            <Register />
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default LoginContent
