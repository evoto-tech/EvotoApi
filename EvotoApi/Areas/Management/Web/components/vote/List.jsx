import React from 'react'
import { Link } from 'react-router'

class VoteList extends React.Component {
  constructor (props) {
    super(props)
    this.state = { votes: [], loaded: false, toDelete: {} }
  }

  componentDidMount () {
    this.fetchVotes()
  }

  fetchVotes () {
    fetch('/mana/vote/list/user')
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
      , { method: 'DELETE' })
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
            <td>{vote.name}</td>
            <td>{vote.creationDate}</td>
            <td>{vote.expiryDate}</td>
            <td><span className={'badge ' + (vote.state === 'published' ? 'bg-green' : 'bg-red')}>{vote.state}</span></td>
            <td>{vote.state !== 'published' ? <Link to={`/vote/${vote.id}/edit`}><i className='fa fa-edit' /></Link> : ''}</td>
            <td>{vote.state !== 'published' ? <div onClick={this.handleDelete.bind(this, vote)}><i className='fa fa-trash' /></div> : ''}</td>
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
      <div className='box'>
        { !this.state.loaded ? (
          <div className='overlay'>
            <i className='fa fa-refresh fa-spin' />
          </div>
          ) : ''
        }
        <div className='box-header with-border'>
          <h3 className='box-title'>Votes</h3>
        </div>
        <div className='box-body'>
          <table className='table table-bordered'>
            <tbody><tr>
              <th style={{width: '10px'}}>#</th>
              <th>Task</th>
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
