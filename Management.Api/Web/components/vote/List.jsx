import React from 'react'
import { Link } from 'react-router'
import formatDateString from '../../lib/format-date-string'
import LoadableOverlay from '../parts/LoadableOverlay.jsx'

class VoteList extends React.Component {
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

  handleDelete (vote) {
    this.setState({ toDelete: vote }, () => {
      this.swalDeleteAlert(vote)
    })
  }

  swalDeleteAlert (vote) {
    swal({
      title: 'Are you sure?',
      text: `'${vote.name}' will be deleted permanently. This cannot be undone.`,
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#DD6B55',
      confirmButtonText: 'Delete',
      closeOnConfirm: false,
      allowOutsideClick: true,
      showLoaderOnConfirm: true
    },
    () => {
      this.confirmDelete(vote)
    })
  }

  confirmDelete (vote) {
    this.setState({ loaded: false }, () => {
      this.performDelete(vote)
    })
  }

  performDelete (vote) {
    fetch(`/mana/vote/${vote.id}/delete`
      , { method: 'DELETE', credentials: 'same-origin' })
        .then((res) => res.json())
        .then((data) => {
          if (data === 1) {
            this.setState({ toDelete: {} }, () => {
              this.fetchVotes()
              swal(`${vote.name} has been deleted.`)
            })
          } else {
            this.setState({ toDelete: {}, loaded: true }, () => {
              console.error('Unexpected result', data)
            })
          }
        })
        .catch(console.error)
  }

  createVoteRows () {
    return this.state.votes.length > 0 ? (
      this.state.votes.map((vote, i) => {
        return (
          <tr key={i}>
            <td>{i + 1}.</td>
            <td><Link to={vote.published ? `/vote/${vote.id}` : `/vote/${vote.id}/edit`}>{vote.name}</Link></td>
            <td>{formatDateString(vote.creationDate)}</td>
            <td>{formatDateString(vote.expiryDate)}</td>
            <td><span className={'badge ' + (vote.published ? 'bg-green' : 'bg-red')}>{vote.published ? 'Published' : 'Draft'}</span></td>
            <td>{!vote.published ? <Link to={`/vote/${vote.id}/edit`}><i className='fa fa-edit' /></Link> : ''}</td>
            <td>{!vote.published ? <div onClick={this.handleDelete.bind(this, vote)}><i className='fa fa-trash' /></div> : ''}</td>
          </tr>
        )
      })
    ) : (
      <tr>
        <td colSpan='7'>No votes!</td>
      </tr>
    )
  }

  render () {
    return (
      <div className='box box-success'>
        <LoadableOverlay loaded={this.state.loaded} />
        <div className='box-header with-border'>
          <h3 className='box-title'>Votes</h3>
          <div className='box-tools pull-right'>
            <button className='btn btn-box-tool' data-widget='collapse'><i className='fa fa-minus' /></button>
          </div>
        </div>
        <div className='box-body'>
          <table className='table table-bordered'>
            <tbody><tr>
              <th style={{width: '10px'}}>#</th>
              <th>Vote Name</th>
              <th style={{width: '200px'}}>Created on</th>
              <th style={{width: '200px'}}>Expires on</th>
              <th style={{width: '40px'}}>State</th>
              <th style={{width: '20px'}} />
              <th style={{width: '20px'}} />
            </tr>
              {this.createVoteRows()}
            </tbody></table>
        </div>
      </div>
    )
  }
}

export default VoteList
