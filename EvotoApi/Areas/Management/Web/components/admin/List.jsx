import React from 'react'
import Wrapper from './parts/Wrapper.jsx'
import LoadableOverlay from '../parts/LoadableOverlay.jsx'

class AdminList extends React.Component {
  constructor (props) {
    super(props)
    this.state = { admins: [], loaded: false, toDelete: {} }
  }

  componentDidMount () {
    this.fetchAdmins()
  }

  fetchAdmins () {
    fetch('/mana/user/list', { credentials: 'same-origin' })
      .then((res) => res.json())
      .then((data) => {
        this.setState({ admins: data, loaded: true })
      })
      .catch(console.error)
  }

  createAdminRows () {
    return this.state.admins.length > 0 ? (
      this.state.admins.map((admin, i) => {
        return (
          <tr key={i}>
            <td>{i + 1}.</td>
            <td>{admin.email}</td>
            <td />
            <td />
          </tr>
        )
      })
    ) : (
      <tr>
        <td colSpan='4'>No administrators! How are you here?</td>
      </tr>
    )
  }

  render () {
    return (
      <Wrapper
        title='Administrators Management'
        description='Manage administrators for the management site.'
        >
        <div className='box box-success'>
          <LoadableOverlay loaded={this.state.loaded} />
          <div className='box-header with-border'>
            <h3 className='box-title'>Administrators</h3>
          </div>
          <div className='box-body'>
            <table className='table table-bordered'>
              <tbody><tr>
                <th style={{width: '10px'}}>#</th>
                <th>Email</th>
                <th style={{width: '200px'}}>Verified</th>
                <th style={{width: '20px'}} />
              </tr>
                {this.createAdminRows()}
              </tbody>
            </table>
          </div>
        </div>
      </Wrapper>
    )
  }
}

export default AdminList
