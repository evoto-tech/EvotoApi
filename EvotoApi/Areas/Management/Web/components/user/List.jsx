import React from 'react'
import { Link } from 'react-router'
import Wrapper from './parts/Wrapper.jsx'
import LoadableOverlay from '../parts/LoadableOverlay.jsx'
import formatDateString from '../../lib/format-date-string'

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

  createUserRows () {
    return this.state.users.length > 0 ? (
      this.state.users.map((user, i) => {
        return (
          <tr key={i}>
            <td>{i + 1}.</td>
            <td>{user.email}</td>
            <td><span className={'badge ' + (user.emailConfirmed ? 'bg-green' : 'bg-red')}>{user.emailConfirmed ? 'Verified' : 'Unverified'}</span></td>
            <td>{formatDateString(user.creationDate)}</td>
            <td><Link to={`/users/${user.id}`}><i className='fa fa-edit' /></Link></td>
          </tr>
        )
      })
    ) : (
      <tr>
        <td colSpan='5'>No users!</td>
      </tr>
    )
  }

  render () {
    let title = 'List',
        description = 'User List'
    return (
      <Wrapper title={title} description={description}>
        <div className='box'>
          <LoadableOverlay loaded={this.state.loaded} />
          <div className='box-header with-border'>
            <h3 className='box-title'>Users</h3>
          </div>
          <div className='box-body'>
            <table className='table table-bordered'>
              <tbody><tr>
                <th style={{width: '10px'}}>#</th>
                <th>Email</th>
                <th style={{width: '200px'}}>Created on</th>
                <th style={{width: '200px'}}>Verified</th>
                <th style={{width: '20px'}} />
              </tr>
                {this.createUserRows()}
              </tbody></table>
          </div>
        </div>
      </Wrapper>
    )
  }
}

export default UserList
