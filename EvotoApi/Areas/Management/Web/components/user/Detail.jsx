import React from 'react'
import Wrapper from './parts/Wrapper.jsx'

class UserDetail extends React.Component {
  constructor (props) {
    super(props)
    this.state = { user: {}, loaded: false }
  }

  componentDidMount () {
    fetch(`/regi/user/detail/${this.props.params.id}`)
      .then((res) => res.json())
      .then((data) => {
        this.setState({ user: data, loaded: true })
      })
      .catch(console.error)
  }

  render () {
    let title = 'Details',
        description = 'User details'
    return (
      <Wrapper title={title} description={description}>
        <div className='box'>
          { !this.state.loaded ? (
            <div className='overlay'>
              <i className='fa fa-refresh fa-spin' />
            </div>
            ) : ''
          }
          <div className='box-header with-border'>
            <h3 className='box-title'>User Details</h3>
          </div>
          <div className='box-body'>
            Email: {this.state.user.email}
          </div>
        </div>
      </Wrapper>
    )
  }
}

export default UserDetail
