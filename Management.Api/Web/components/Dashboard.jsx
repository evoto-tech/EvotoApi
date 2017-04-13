import React from 'react'
import { Link } from 'react-router'
import PropTypes from 'prop-types'
import formatDate from '../lib/format-date-string'

class Dashboard extends React.Component {
  constructor (props) {
    super(props)
    this.state = { votes: [], loaded: false, toDelete: {} }
  }

  componentDidMount () {
    this.fetchVotes()
  }

  fetchVotes () {
    fetch('/mana/vote/list', { credentials: 'same-origin' })
      .then((res) => res.json())
      .then((data) => {
        this.setState({ votes: data, loaded: true })
      })
      .catch(console.error)
  }

  getCurrentVotes () {
    return this.state.votes
      .filter((vote) => {
        return vote.published && moment().isBetween(moment(vote.publishedDate), moment(vote.expiryDate))
      })
  }

  getDaysPercentage (daysOfVote, daysUntilExpiry) {
    let daysPercentage = ((daysOfVote - daysUntilExpiry) / daysOfVote) * 100
    if (isNaN(daysPercentage)) daysPercentage = 0
    daysPercentage = (daysPercentage < 0 ? 0 : daysPercentage)
    daysPercentage = (daysPercentage > 100 ? 100 : daysPercentage)
    return daysPercentage
  }

  getCurrentVotesInfoBoxes (currentVotes) {
    return currentVotes.length > 0 ? (
      currentVotes.map((vote, i) => {
        const publishedDate = vote.publishedDate ? moment(vote.publishedDate) : moment()
        const expiryDate = moment(vote.expiryDate)
        const daysOfVote = expiryDate.diff(publishedDate, 'days')
        const daysUntilExpiry = expiryDate.diff(moment(), 'days')
        const daysPercentage = this.getDaysPercentage(daysOfVote, daysUntilExpiry)
        return (
          <Link to={`/vote/${vote.id}`} key={i}>
            <div className='info-box bg-green'>
              <span className='info-box-icon'><i className='fa fa-check' /></span>
              <div className='info-box-content'>
                <span className='info-box-text'>{vote.name}</span>
                <span className='info-box-number'>{formatDate(vote.publishedDate)} until {formatDate(vote.expiryDate)}</span>
                <div className='progress'>
                  <div className='progress-bar' style={{ width: `${daysPercentage}%` }} />
                </div>
                <span className='progress-description'>
                  <b>{daysUntilExpiry} days left</b>
                </span>
              </div>
            </div>
          </Link>
        )
      })
    ) : (
      'No currently active published votes available!'
    )
  }

  render () {
    const currentVotes = this.getCurrentVotes()
    return (
      <div className='box box-success'>
        { !this.state.loaded ? (
          <div className='overlay'>
            <i className='fa fa-refresh fa-spin' />
          </div>
          ) : ''
        }
        <div className='box-header with-border'>
          <h3 className='box-title'>{currentVotes.length} Active Vote{currentVotes.length !== 1 ? 's' : ''}</h3>
          <div className='box-tools pull-right'>
            <button className='btn btn-box-tool' data-widget='collapse'><i className='fa fa-minus' /></button>
          </div>
        </div>
        <div className='box-body'>
          {this.getCurrentVotesInfoBoxes(currentVotes)}
        </div>
      </div>
    )
  }
}

export default Dashboard
