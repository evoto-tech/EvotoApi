import React from 'react'
import Wrapper from './parts/Wrapper.jsx'

class UserNew extends React.Component {
  render () {
    let title = 'New User'
    let description = 'New User'
    return (
      <Wrapper title={title} description={description} />
    )
  }
}

export default UserNew
