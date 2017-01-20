import React from 'react'
import {IndexLink} from 'react-router'

class VoteList extends React.Component {
  constructor (props) {
    super(props)
    this.state = { votes: [], loaded: false }
  }

  componentDidMount () {
    fetch('/mana/vote/list/org')
      .then((res) => res.json())
      .then((data) => {
        this.setState({ votes: data, loaded: true })
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
            <td><span className="badge bg-red">{vote.state}</span></td>
          </tr>
        )
      })
    ) : (
      <tr>
        <td colSpan="5">No votes!</td>
      </tr>
    )
  }

  render () {
    return (
      <div className="box">
        { !this.state.loaded ? (
            <div className="overlay">
              <i className="fa fa-refresh fa-spin"></i>
            </div>
          ) : ''
        }
        <div className="box-header with-border">
          <h3 className="box-title">Votes</h3>
        </div>
        <div className="box-body">
          <table className="table table-bordered">
            <tbody><tr>
              <th style={{width: "10px"}}>#</th>
              <th>Task</th>
              <th style={{width: "200px"}}>Created on</th>
              <th style={{width: "200px"}}>Expires on</th>
              <th style={{width: "40px"}}>State</th>
            </tr>
            {this.createVoteRows()}
          </tbody></table>
        </div>
      </div>
    )
  }
}

export default VoteList
