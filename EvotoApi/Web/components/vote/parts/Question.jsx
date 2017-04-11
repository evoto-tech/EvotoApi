import React from 'react'
import Answer from './Answer.jsx'
import { insert, update, remove } from '../../../lib/state-utils'

class Question extends React.Component {
  constructor (props) {
    super(props)
    this.state = Object.assign({}, this.stateFromProps(props))
  }

  propTypes: {
    id: React.PropTypes.number,
    question: React.PropTypes.object,
    onDelete: React.PropTypes.func,
    onChange: React.PropTypes.func,
    disabled: React.PropTypes.bool
  }

  stateFromProps (props) {
    return {
      question: props.question.question || '',
      answers: props.question.answers || [],
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

  addAnswer () {
    this.setState({ answers: insert(this.state.answers, { answer: '', info: '' }) }, this.onChange)
  }

  deleteAnswer (index) {
    this.setState({ answers: remove(this.state.answers, index) }, this.onChange)
  }

  updateAnswer (index, answer) {
    this.setState({ answers: update(this.state.answers, index, answer) }, this.onChange)
  }

  updateField (field, e) {
    this.setState({ [field]: e.target.value }, this.onChange)
  }

  render () {
    return (
      <div className='box box-success box-solid'>
        <div className='box-header with-border'>
          <h4 className='box-title'>
            <div className='input-group' style={{ padding: '2px' }}>
              <span className='input-group-addon' style={{ color: '#000000' }}>Question {this.props.id + 1}</span>
              <input
                type='text'
                className='form-control'
                placeholder='Question...'
                value={this.state.question}
                onChange={this.updateField.bind(this, 'question')}
                disabled={this.props.disabled}
              />
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
              onChange={this.updateField.bind(this, 'info')}
              value={this.state.info}
              disabled={this.props.disabled}
              />
          </div>
          <div className='form-group'>
            <label>Options</label>
            {this.state.answers.map((answer, i) => (
              <Answer
                key={i}
                id={i}
                answer={answer}
                onDelete={this.deleteAnswer.bind(this)}
                onChange={this.updateAnswer.bind(this, i)}
                disabled={this.props.disabled}
                />
            ))}
          </div>
          {this.props.disabled ? '' : (
            <div className='btn-group'>
              <button type='button' className='btn btn-danger' onClick={this.delete.bind(this)}>Delete Question</button>
              <button type='button' className='btn btn-success' onClick={this.addAnswer.bind(this)}>Add Option</button>
            </div>
          )}
        </div>
      </div>
    )
  }
}

export default Question
