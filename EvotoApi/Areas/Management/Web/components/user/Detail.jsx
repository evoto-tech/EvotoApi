import React from 'react'
import Wrapper from './parts/Wrapper.jsx'

class UserDetail extends React.Component {
  render () {
    let title = 'Details',
        description = 'User details'
    return (
      <Wrapper title={title} description={description} />
    )
  }
}

export default UserDetail
