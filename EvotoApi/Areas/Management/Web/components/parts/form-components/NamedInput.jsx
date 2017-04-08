import React from 'react'

const propTypes = {
  name: React.PropTypes.node,
  placeholder: React.PropTypes.string,
  type: React.PropTypes.string,
  inputGroupStyle: React.PropTypes.object,
  spanStyle: React.PropTypes.object,
  inputStyle: React.PropTypes.object,
  inputRef: React.PropTypes.func
}

class NamedInput extends React.Component {
  render () {
    const inputGroupStyle = Object.assign({}, { padding: '2px', width: '100%' }, this.props.inputGroupStyle)
    const spanStyle = Object.assign({}, { color: '#000000', width: '130px', userSelect: 'none' }, this.props.spanStyle)
    const inputStyle = Object.assign({}, { width: '100%' }, this.props.inputStyle)
    const span = (
      <span className='input-group-addon' style={spanStyle}>
        {this.props.name}
      </span>
      )
    const props = Object.keys(this.props).reduce((props, propKey) => {
      if (Object.keys(propTypes).includes(propKey)) return props
      return Object.assign(props, { [propKey]: this.props[propKey] })
    }, {})
    const input = (
      <input
        {...props}
        type={this.props.type || 'text'}
        className={this.props.className || 'form-control'}
        placeholder={`${this.props.placeholder || this.props.name + '...'}`}
        style={inputStyle}
        ref={this.props.inputRef}
        />
      )
    const labeledTypes = [ 'checkbox', 'radio' ]
    if (labeledTypes.includes(this.props.type)) {
      return (
        <div className='input-group' style={inputGroupStyle}>
          <label>
            {span}
            {input}
          </label>
        </div>
      )
    } else {
      return (
        <div className='input-group' style={inputGroupStyle}>
          {span}
          {input}
        </div>
      )
    }
  }
}

NamedInput.propTypes = propTypes

export default NamedInput
