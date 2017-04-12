import React from 'react'
import {Link, IndexLink} from 'react-router'

class Sidebar extends React.Component {
  propTypes: {
    user: React.PropTypes.object
  }

  render () {
    const userImage = this.props.user && this.props.user.picture ? (
      <div className='pull-left image'>
        <img className='img-circle' alt='User Image' src='{this.props.user.picture}' />
      </div>
    ) : (
      <div className='pull-left placeholder__user-picture'>
        <div style={{ width: '100%', textAlign: 'center' }}><div><i className='fa fa-user' /></div></div>
      </div>
    )

    return (
      <aside className='main-sidebar'>

        <section className='sidebar' style={{height: '100%'}}>

          <div className='user-panel'>
            {userImage}
            <div className='pull-left info'>
              <p>Administrator</p>
            </div>
          </div>

          <ul className='sidebar-menu'>
            <li className='header'>MENU</li>
            <li><IndexLink to='/' activeClassName='active'><i className='fa fa-home' /><span>Home</span></IndexLink></li>
            <li><Link to='/' activeClassName='active'><i className='fa fa-envelope' /><span>Votes</span></Link></li>
            <li><Link to='/admins' activeClassName='active'><i className='fa fa-users' /><span>Administrators</span></Link></li>
            <li><Link to='/users' activeClassName='active'><i className='fa fa-users' /><span>Users</span></Link></li>
            <li><Link to='/settings' activeClassName='active'><i className='fa fa-cogs' /><span>Settings</span></Link></li>
          </ul>
        </section>
        <div style={{height: '100%'}} />
      </aside>
    )
  }
}

Sidebar.propTypes = {

}

export default Sidebar
