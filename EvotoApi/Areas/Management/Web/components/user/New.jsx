import React from 'react'
import Wrapper from './parts/Wrapper.jsx'

class UserDetail extends React.Component {
  render () {
    let title = 'New User',
        description = 'New User'
    return (
      <Wrapper title={title} description={description}>

      </Wrapper>
    )
  }
}

export default UserDetail
