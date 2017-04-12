import React from 'react'
import NewVote from './New.jsx'
import Results from './parts/Results.jsx'

class EditVote extends React.Component {
  constructor (props) {
    super(props)
    this.state = { vote: {}, loaded: false }
  }

  componentDidMount () {
    fetch(`/mana/vote/${this.props.params.id}`, { credentials: 'same-origin' })
      .then((res) => res.json())
      .then((data) => {
        this.setState({ vote: data, loaded: true }, () => {
          if (this.results) this.results.load(this.state.vote)
        })
      })
      .catch(console.error)
  }

  render () {
    return (
      <div>
        <NewVote
          title={'View Vote'}
          description={'View an existing vote'}
          loaded={this.state.loaded}
          vote={this.state.vote}
          save={() => {}}
          disabled
        >
          {!this.state.vote || !this.state.vote.published ? '' :
            <Results
              loaded={this.state.loaded}
              vote={this.state.vote}
              ref={(results) => { this.results = results }}
            />
          }
        </NewVote>
      </div>
    )
  }
}

export default EditVote
