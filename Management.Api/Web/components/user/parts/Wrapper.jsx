import React from 'react'
import { Link } from 'react-router'
import PropTypes from 'prop-types'
import Wrapper from '../../parts/Wrapper.jsx'

class UserWrapper extends React.Component {
  render () {
    let title = this.props.title || ''
    let description = this.props.description || ''
    return (
      <Wrapper title={title} description={description} breadcrumbs={(
        [<Link to='/users/new'><i className='fa fa-plus' />New User</Link>]
      )}>
        {this.props.children}
      </Wrapper>
    )
  }
}

UserWrapper.propTypes = {
  title: PropTypes.string.isRequired,
  description: PropTypes.string.isRequired
}

export default UserWrapper
