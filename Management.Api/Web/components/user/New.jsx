import React from 'react'
import Wrapper from './parts/Wrapper.jsx'
import RegisterForm from '../parts/forms/Register.jsx'

class UserNew extends React.Component {
  register (user, successLink, handleError) {
    console.log('what', user)
  }

  render () {
    let title = 'New User'
    let description = 'New User'
    return (
      <Wrapper title={title} description={description}>
        <div className='box box-success'>
          <div className='box-header with-border'>
            <h3 className='box-title'>New User Details</h3>
          </div>
          <div className='box-body'>
            <RegisterForm
              onProcess={this.register.bind(this)}
              successLink='/users'
              buttonText='Create New User'
            />
          </div>
        </div>
      </Wrapper>
    )
  }
}

export default UserNew
