import React from 'react'
import { withRouter, Link } from 'react-router'

class UserWrapper extends React.Component {
  propTypes: {
      title: React.PropTypes.string.isRequired,
      description: React.PropTypes.string.isRequired
  }

  render () {
    let title = this.props.title || '',
        description = this.props.description || ''
    return (
      <div className='content-wrapper' style={{ height: '100%' }}>
        <section className='content-header' style={{ height: '100%' }}>
          <h1>{title}<small>{description}</small></h1>
          <ol className='breadcrumb'>
            <li>
              <Link to='/users/new'><i className='fa fa-plus' />New User</Link>
            </li>
          </ol>
        </section>
        <section className='content'>
          {this.props.children}
        </section>
      </div>
    )
  }
}

export default UserWrapper
