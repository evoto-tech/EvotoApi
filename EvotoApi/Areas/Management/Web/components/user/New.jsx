import React from 'react'
import Wrapper from './parts/Wrapper.jsx'

class UserNew extends React.Component {
  render () {
    let title = 'New User',
        description = 'New User'
    return (
      <Wrapper title={title} description={description}>

      </Wrapper>
    )
  }
}

export default UserNew
