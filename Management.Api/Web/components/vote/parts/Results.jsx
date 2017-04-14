import qs from 'querystring'
import React from 'react'
import Please from 'pleasejs'
import LoadableOverlay from '../../parts/LoadableOverlay.jsx'
import Chart from '../../parts/Chart.jsx'
import formatDateString from '../../../lib/format-date-string'

class Option extends React.Component {
  constructor (props) {
    super(props)
    this.state = this.stateFromProps(props)
  }

  stateFromProps (props) {
    return {
      loaded: this.state ? (this.state.loaded || false) : false,
      loadedVote: props.loaded || false,
      results: this.state ? (this.state.results || []) : [],
      error: this.state ? (this.state.error || '') : '',
    }
  }

  load (vote) {
    this.setState({ loadedVote: true }, this.fetchResults.bind(this))
  }

  fetchResults () {
    if (this.state.loadedVote && this.props.vote) {
      fetch(`/regi/results?${qs.stringify({ chainString: this.props.vote.chainString })}`,
        { credentials: 'same-origin'
        })
        .then((res) => res.json())
        .then((data) => {
          if (data.Message) {
            this.setState({ error: data.Message, loaded: true })
          } else {
            this.setState({ results: data, loaded: true })
          }
        })
        .catch((err) => {
          console.error(err)
          this.setState({ error: 'There was an error retrieving the results, please try again.' })
        })
    }
  }

  formatError () {
    const error = this.state.error
    if (error === 'Encrypted results') {
      return `The results for this vote are still encrypted, results will be available after ${formatDateString(this.props.vote.expiryDate)}`
    }
    return error
  }

  formatQuestions () {
    return this.state.results.map(this.formatQuestion.bind(this))
  }

  formatQuestion (question, i) {
    const questionText = question.question
    const results = question.answers
    const numberOfVotes = Object.keys(results).reduce((total, key) => total += results[key], 0)
    return (
      <div key={i}>
        <h5 style={{ fontWeight: 'bold' }}>{question.number} - {questionText}</h5>
        {!results || numberOfVotes === 0 ? <p>No results to show!</p> : this.formatChart(results)}
      </div>
    )
  }

  formatChart (results) {
    const colors = Please.make_color(
        { format: 'hex',
          colors_returned: (Object.keys(results).length || 0)
        })
    return (
      <Chart type='pie' data={{
        labels: Object.keys(results),
        datasets: [
          {
            data: Object.keys(results).map((k) => results[k]),
            backgroundColor: colors
          }]
      }} options={{}} height={40} />
    )
  }

  render () {
    return (
      <div className='box box-success'>
        <LoadableOverlay loaded={this.state.loaded && this.state.loadedVote} />
        <div className='box-header with-border'>
          <h3 className='box-title'>Vote Results <small>{this.state.results.length} Question{this.state.results.length === 1 ? '' : 's'}</small></h3>
        </div>
        <div className='box-body'>
          {this.state.error === '' ? this.formatQuestions() : this.formatError()}
        </div>
      </div>
    )
  }
}

Option.propTypes = {
  loaded: React.PropTypes.bool,
  vote: React.PropTypes.object
}

export default Option
