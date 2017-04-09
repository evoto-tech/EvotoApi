import React from 'react'
import Chartjs from 'chart.js'

class Chart extends React.Component {
  propTypes: {
    type: React.PropTypes.string,
    data: React.PropTypes.object,
    options: React.PropTypes.object,
    height: React.PropTypes.number,
    width: React.PropTypes.number
  }

  componentDidMount () {
    let chartCanvas = this.refs.chart

    let myChart = new Chartjs(chartCanvas, {
      type: this.props.type,
      data: this.props.data,
      options: this.props.options
    })

    this.setState({chart: myChart})
  }

  componentDidUpdate () {
    let chart = this.state.chart
    let data = this.props.data
    data.datasets.forEach((dataset, i) => {
      chart.data.datasets[i].data = dataset.data
    })
    chart.data.labels = data.labels
    chart.update()
  }

  render () {
    return (
      <canvas ref={'chart'} height={this.props.height} width={this.props.width} />
    )
  }
}

export default Chart
