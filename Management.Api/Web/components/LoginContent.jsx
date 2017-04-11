import React from 'react'
import Login from './auth/Login.jsx'

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
                      <Login successLink='/' />
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
