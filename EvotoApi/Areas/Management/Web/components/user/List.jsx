import React from 'react'
import Wrapper from './parts/Wrapper.jsx'

class UserList extends React.Component {
  constructor (props) {
    super(props)
    this.state = { users: [], loaded: false }
  }

  componentDidMount () {
    this.fetchVotes()
  }

  fetchVotes () {
    fetch('/regi/users/list')
      .then((res) => res.json())
      .then((data) => {
        this.setState({ users: data, loaded: true })
      })
      .catch(console.error)
  }

  render () {
    let title = 'List',
        description = 'User List'
    return (
      <Wrapper title={title} description={description} />
    )
  }
}

export default UserList
