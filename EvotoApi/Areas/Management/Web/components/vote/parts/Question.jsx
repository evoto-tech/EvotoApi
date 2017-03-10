import React from 'react'

class Question extends React.Component {
  constructor (props) {
    super(props)
    this.state = Object.assign({}, this.stateFromProps(props))
  }

  propTypes: {
    id: React.PropTypes.number,
    question: React.PropTypes.object,
    onDelete: React.PropTypes.func,
    onChange: React.PropTypes.func
  }

  stateFromProps (props) {
    return {
      question: props.question.question || '',
      options: props.question.options || []
    }
  }

  componentWillReceiveProps (nextProps) {
    this.setState(this.stateFromProps(nextProps))
  }

  delete () {
    this.props.onDelete(this.props.id)
  }

  onChange () {
    this.props.onChange(this.state)
  }

  addOption () {
    let options = [].concat(this.state.options)
    options.push('')
    this.setState({ options }, this.onChange)
  }

  deleteOption (index) {
    let options = [].concat(this.state.options)
    options.splice(index, 1)
    this.setState({ options }, this.onChange)
  }

  updateQuestion(e) {
    this.setState({ question: e.target.value }, this.onChange)
  }

  updateOption(index, e) {
    let options = [].concat(this.state.options)
    options[index] = e.target.value
    this.setState({ options }, this.onChange)
  }

  render () {
    return (
      <div className='callout callout-success'>
        <h4>Question {this.props.id + 1}</h4>
        <div className='form-group'>
          <label>Question</label>
          <input type='text' className='form-control' placeholder='Question...' value={this.state.question} onChange={this.updateQuestion.bind(this)} />
        </div>
        <div className='form-group'>
          <label>Options</label>
          {this.state.options.map((option, i) => (
            <div className='input-group' key={i} style={{ padding: '2px' }}>
              <span className='input-group-addon' style={{ color: '#000000' }}>{i + 1}</span>
              <input type='text' className='form-control' placeholder='Option...' value={this.state.options[i]} onChange={this.updateOption.bind(this, i)}/>
              <span className='input-group-btn'>
                <button type='button' className='btn btn-danger btn-flat' onClick={this.deleteOption.bind(this, i)}>
                  <i className='fa fa-times'></i>
                </button>
              </span>
            </div>
          ))}
        </div>
        <div className='btn-group'>
          <button type='button' className='btn btn-danger' onClick={this.delete.bind(this)}>Delete Question</button>
          <button type='button' className='btn btn-success' onClick={this.addOption.bind(this)}>Add Option</button>
        </div>
      </div>
    )
  }
}

export default Question
