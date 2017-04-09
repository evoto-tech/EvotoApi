import React from 'react'
import LoadableOverlay from '../../parts/LoadableOverlay.jsx'
import Chart from '../../parts/Chart.jsx'

class Option extends React.Component {
  propTypes: {
    loaded: React.PropTypes.bool,
    vote: React.PropTypes.object
  }

  render () {
    let questions = this.props.vote && this.props.vote.questions ? JSON.parse(this.props.vote.questions) : []
    return (
      <div className='box box-success'>
        <LoadableOverlay loaded={this.props.loaded} />
        <div className='box-header with-border'>
          <h3 className='box-title'>Vote Results <small>{questions.length} Questions</small></h3>
        </div>
        <div className='box-body'>
          {questions.map((question, i) => (
            <div key={i}>
              <h5>Results for '{question.question}'</h5>
              <Chart type='pie' data={{
                labels: [
                  'Red',
                  'Blue',
                  'Yellow'
                ],
                datasets: [
                  {
                    data: [300, 50, 100],
                    backgroundColor: [
                      '#FF6384',
                      '#36A2EB',
                      '#FFCE56'
                    ],
                    hoverBackgroundColor: [
                      '#FF6384',
                      '#36A2EB',
                      '#FFCE56'
                    ]
                  }]
              }} options={{}} height={40} />
            </div>
          ))}
        </div>
      </div>
    )
  }
}

export default Option
