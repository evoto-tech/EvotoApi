import React from 'react'
import { withRouter } from 'react-router'
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

  save (vote, postSave, showErrors) {
    fetch(`/mana/vote/${this.state.vote.id}/edit`
      , { method: 'PATCH',
        body: JSON.stringify(vote),
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      })
      .then((data) => {
        return data.json()
      })
      .then((json) => {
        if (json.errors) {
          showErrors(json.errors)
        } else {
          postSave()
        }
      })
      .catch((err) => {
        console.error(err)
      })
  }

  render () {
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
