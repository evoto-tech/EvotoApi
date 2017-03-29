import React from 'react'

class Dashboard extends React.Component {
  constructor (props) {
    super(props)
    this.state = { votes: [], loaded: false, toDelete: {} }
  }

  componentDidMount () {
    this.fetchVotes()
  }

  fetchVotes () {
    fetch('/mana/vote/list/user/2', { credentials: 'same-origin' })
      .then((res) => res.json())
      .then((data) => {
        this.setState({ votes: data, loaded: true })
      })
      .catch(console.error)
  }

  getCurrentVotes () {
    return this.state.votes
      .filter((vote) => {
        return vote.published && moment().isBetween(moment(vote.creationDate), moment(vote.expiryDate))
      })
  }

  getCurrentVotesInfoBoxes (currentVotes) {
    return currentVotes.length > 0 ? (
      currentVotes.map((vote, i) => {
        return (
          <div className='info-box bg-green' key={i}>
            <span className='info-box-icon'><i className='fa fa-check' /></span>
            <div className='info-box-content'>
              <span className='info-box-text'>{vote.name}</span>
              <span className='info-box-number'># votes used</span>
              <div className='progress'>
                <div className='progress-bar' style={{ width: '70%' }} />
              </div>
              <span className='progress-description'>
                {moment(vote.expiryDate).diff(moment(), 'days')} days left
              </span>
            </div>
          </div>
        )
      })
    ) : (
      'No currently active published votes available!'
    )
  }

  render () {
    const currentVotes = this.getCurrentVotes()
    return (
      <div className='box'>
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
