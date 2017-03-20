import React from 'react'
import { withRouter, Link } from 'react-router'

class UserDetail extends React.Component {
  render () {
    let title = 'Details',
        description = 'User details'
    return (
      <div className='content-wrapper' style={{ height: '100%' }}>
        <section className='content-header' style={{ height: '100%' }}>
          <h1>{title}<small>{description}</small></h1>
          <ol className='breadcrumb'>
            <li>
              <Link to='/user/new'><i className='fa fa-plus' />New User</Link>
            </li>
          </ol>
        </section>
        <section className='content'>
        </section>
      </div>
    )
  }
}

export default UserDetail
