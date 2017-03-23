import React from 'react'
import { withRouter, Link } from 'react-router'
import Wrapper from '../../parts/Wrapper.jsx'

class UserWrapper extends React.Component {
  propTypes: {
    title: React.PropTypes.string.isRequired,
    description: React.PropTypes.string.isRequired
  }

  render () {
    let title = this.props.title || '',
        description = this.props.description || ''
    return (
      <Wrapper title={title} description={description} breadcrumbs={(
        [<Link to='/users/new'><i className='fa fa-plus' />New User</Link>]
      )}>
        {this.props.children}
      </Wrapper>
    )
  }
}

export default UserWrapper
