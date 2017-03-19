import React from 'react'
import Option from './Option.jsx'

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
      options: props.question.options || [],
      info: props.question.info || ''
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
    options.push({ option: '', info: '' })
    this.setState({ options }, this.onChange)
  }

  deleteOption (index) {
    let options = [].concat(this.state.options)
    options.splice(index, 1)
    this.setState({ options }, this.onChange)
  }

  updateQuestion (e) {
    this.setState({ question: e.target.value }, this.onChange)
  }

  updateInfo (e) {
    this.setState({ info: e.target.value }, this.onChange)
  }

  updateOption (index, option) {
    let options = [].concat(this.state.options)
    options[index] = option
    this.setState({ options }, this.onChange)
  }

  render () {
    return (
      <div className='box box-success box-solid'>
        <div className='box-header with-border'>
          <h4 className='box-title'>
            <div className='input-group' style={{ padding: '2px' }}>
              <span className='input-group-addon' style={{ color: '#000000' }}>Question {this.props.id + 1}</span>
              <input type='text' className='form-control' placeholder='Question...' value={this.state.question} onChange={this.updateQuestion.bind(this)} />
            </div>
          </h4>
        </div>
        <div className='box-body'>
          <div className='form-group'>
            <label>Information</label>
            <textarea
              className='form-control'
              rows='2'
              placeholder='Information...'
              style={{ resize: 'vertical' }}
              onChange={this.updateInfo.bind(this)}
              value={this.state.info}
               />
          </div>
          <div className='form-group'>
            <label>Options</label>
            {this.state.options.map((option, i) => (
              <Option
                key={i}
                id={i}
                option={option}
                onDelete={this.deleteOption.bind(this)}
                onChange={this.updateOption.bind(this, i)}
                />
            ))}
          </div>
          <div className='btn-group'>
            <button type='button' className='btn btn-danger' onClick={this.delete.bind(this)}>Delete Question</button>
            <button type='button' className='btn btn-success' onClick={this.addOption.bind(this)}>Add Option</button>
          </div>
        </div>
      </div>
    )
  }
}

export default Question
