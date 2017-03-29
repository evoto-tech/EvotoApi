import React from 'react'

class Option extends React.Component {
  constructor (props) {
    super(props)
    this.state = Object.assign({}, this.stateFromProps(props))
  }

  propTypes: {
    id: React.PropTypes.number,
    option: React.PropTypes.object,
    onDelete: React.PropTypes.func,
    onChange: React.PropTypes.func,
    disabled: React.PropTypes.bool
  }

  stateFromProps (props) {
    return {
      option: props.option.option || '',
      info: props.option.info || ''
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

  updateOption (e) {
    this.setState({ option: e.target.value }, this.onChange)
  }

  updateInfo (e) {
    this.setState({ info: e.target.value }, this.onChange)
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
            value={this.state.option}
            onChange={this.updateOption.bind(this)}
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
            onChange={this.updateInfo.bind(this)}
            value={this.state.info}
            disabled={this.props.disabled}
          />
        </div>
      </div>
    )
  }
}

export default Option
