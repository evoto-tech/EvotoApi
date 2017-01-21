import React from 'react'
import { withRouter, Link } from 'react-router'
import NewVote from './New.jsx'

class EditVote extends React.Component {
  constructor (props) {
    super(props)
    this.state = { vote: {}, loaded: false }
  }

  componentDidMount () {
    fetch(`/mana/vote/${this.props.params.id}`)
      .then((res) => res.json())
      .then((data) => {
        this.setState({ vote: data, loaded: true })
      })
      .catch(console.error)
  }

  save (vote) {
    console.log('editing!', vote)
    fetch('/mana/vote/edit'
      , { method: 'PUT',
        body: JSON.stringify(vote),
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      })
      .then((res) => {
        console.log('res', res)
        this.props.router.push('/')
      })
      .catch((err) => {
        console.error(err)
      })
    this.props.router.push('/')
  }

  render () {
    console.log('state', this.state)
    return (
      <div>
        <NewVote
          title={'Edit Vote'}
          description={'Edit an existing vote'}
          loaded={this.state.loaded}
          vote={this.state.vote}
          save={this.save.bind(this)} />
      </div>
    )
  }
}

export default withRouter(EditVote)
