import React from 'react'
import PropTypes from 'prop-types'

class Answer extends React.Component {
  constructor (props) {
    super(props)
    this.state = Object.assign({}, this.stateFromProps(props))
  }

  stateFromProps (props) {
    return {
      answer: props.answer.answer || '',
      info: props.answer.info || ''
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

  updateField (field, e) {
    this.setState({ [field]: e.target.value }, this.onChange)
  }

  render () {
    return (
      <div>
        <div className='input-group'>
          <span className='input-group-addon' style={{ color: '#000000' }}>{this.props.id + 1}</span>
          <input
            disabled={this.props.disabled}
            type='text'
            className='form-control'
            placeholder='Option...'
            value={this.state.answer}
            onChange={this.updateField.bind(this, 'answer')}
          />
          {this.props.disabled ? '' : (
            <span className='input-group-btn'>
              <button type='button' className='btn btn-danger btn-flat' onClick={this.delete.bind(this)}>
                <i className='fa fa-times' />
              </button>
            </span>
          )}
        </div>
        <div className='form-group'>
          <textarea
            className='form-control'
            rows='2'
            placeholder='Information...'
            style={{ resize: 'vertical', borderTop: 'none' }}
            onChange={this.updateField.bind(this, 'info')}
            value={this.state.info}
            disabled={this.props.disabled}
          />
        </div>
      </div>
    )
  }
}

Answer.propTypes = {
  id: PropTypes.number,
  answer: PropTypes.object,
  onDelete: PropTypes.func,
  onChange: PropTypes.func,
  disabled: PropTypes.bool
}

export default Answer
