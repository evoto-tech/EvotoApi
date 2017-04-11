import qs from 'querystring'
import React from 'react'
import Please from 'pleasejs'
import LoadableOverlay from '../../parts/LoadableOverlay.jsx'
import Chart from '../../parts/Chart.jsx'

class Option extends React.Component {
  constructor (props) {
    super(props)
    this.state = this.stateFromProps(props)
  }

  stateFromProps (props) {
    return {
      loaded: this.state ? (this.state.loaded || false) : false,
      loadedVote: props.loaded || false,
      results: this.state ? (this.state.results || []) : []
    }
  }

  load (vote) {
    this.setState({ loadedVote: true }, this.fetchResults.bind(this))
  }

  fetchResults () {
    if (this.state.loadedVote && this.props.vote) {
      fetch(`/regi/results?${qs.stringify({ blockchainName: this.props.vote.name })}`,
        { credentials: 'same-origin'
        })
        .then((res) => res.json())
        .then((data) => {
          this.setState({ results: data, loaded: true })
        })
        .catch(console.error)
    }
  }

  render () {
    return (
      <div className='box box-success'>
        <LoadableOverlay loaded={this.state.loaded && this.state.loadedVote} />
        <div className='box-header with-border'>
          <h3 className='box-title'>Vote Results <small>{this.state.results.length} Questions</small></h3>
        </div>
        <div className='box-body'>
          {this.state.results.map((question, i) => {
            const colors = Please.make_color(
              { format: 'hex',
                colors_returned: (Object.keys(question.Results).length || 0)
              })
            return (
              <div key={i}>
                <h5>Results for '{question.Question}'</h5>
                <Chart type='pie' data={{
                  labels: Object.keys(question.Results),
                  datasets: [
                    {
                      data: Object.keys(question.Results).map((k) => question.Results[k]),
                      backgroundColor: colors
                    }]
                }} options={{}} height={40} />
              </div>
            )
          })}
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
