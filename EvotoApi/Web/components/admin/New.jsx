import React from 'react'
import Wrapper from './parts/Wrapper.jsx'
import Register from '../auth/Register.jsx'

class AdminNew extends React.Component {
  propTypes: {

  }

  render () {
    return (
      <Wrapper
        title='New Administrator'
        description='Add a new user to the management site.'
        >
        <div className='box box-success'>
          <div className='box-header with-border'>
            <h3 className='box-title'>New Administrator Details</h3>
          </div>
          <div className='box-body'>
            <Register successLink='/manage/users/list' buttonText='Create New' />
          </div>
        </div>
      </Wrapper>
    )
  }
}

export default AdminNew
