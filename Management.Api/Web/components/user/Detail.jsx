import React from 'react'
import Wrapper from './parts/Wrapper.jsx'
import LoadableOverlay from '../parts/LoadableOverlay.jsx'

class UserDetail extends React.Component {
  constructor (props) {
    super(props)
    this.state = { user: {}, loaded: false }
  }

  componentDidMount () {
    fetch(`/regi/users/detail/${this.props.params.id}`, { credentials: 'same-origin' })
      .then((res) => res.json())
      .then((data) => {
        this.setState({ user: data, loaded: true })
      })
      .catch(console.error)
  }

  render () {
    let title = 'Details'
    let description = 'User details'
    let user = this.state.user
    let customFields = user.customFields
    return (
      <Wrapper title={title} description={description}>
        <div className='box box-success'>
          <LoadableOverlay loaded={this.state.loaded} />
          <div className='box-header with-border'>
            <h3 className='box-title'>User Details</h3>
          </div>
          <div className='box-body'>
            <div className='form-group'><b>Email:</b> {user.email}</div>
            <div className='form-group'><b>Confirmed Email:</b> <span className={'badge ' + (user.emailConfirmed ? 'bg-green' : 'bg-red')}>{user.emailConfirmed ? 'Confirmed' : 'Unconfirmed'}</span></div>
            {customFields && Object.keys(customFields).length > 0 ? (
              Object.keys(customFields).map((field, i) => (
                <div className='form-group' key={i}><b>{field}:</b> {customFields[field]}</div>
              ))
            ) : ''}
          </div>
        </div>
      </Wrapper>
    )
  }
}

export default UserDetail
