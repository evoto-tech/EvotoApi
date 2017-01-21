import React from 'react'
import {IndexLink} from 'react-router'
import VoteList from './vote/List.jsx'

class Home extends React.Component {
  render () {
    return (
      <div className='content-wrapper' style={{ height: '100%' }}>
        <section className='content-header' style={{ height: '100%' }}>
          <h1>evoto Management<small>Manage votes and stuff</small></h1>
          <ol className='breadcrumb'>
            <li>
              <IndexLink to='/vote/new'><i className='fa fa-plus' />New Vote</IndexLink>
            </li>
          </ol>
        </section>
        <section className='content'>
          <VoteList />
        </section>
      </div>
    )
  }
}

export default Home
