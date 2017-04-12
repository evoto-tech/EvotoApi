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
  baseInputGroupStyle: {
    padding: '2px', width: '100%'
  }

  baseSpanStyle: {
    color: '#000000',
    width: '130px',
    userSelect: 'none'
  }

  baseInputStyle: {
    width: '100%'
  }

  render () {
    let baseInputGroupStyle = this.baseInputGroupStyle
    let baseSpanStyle = this.baseSpanStyle
    let baseInputStyle = this.baseInputStyle

    if (this.props.type === 'date') {
      baseInputGroupStyle = Object.assign({}, baseInputGroupStyle, {
        overflow: 'hidden',
        maxHeight: '34px',
        width: '100%'
      })
      baseSpanStyle = Object.assign({}, baseSpanStyle, {
        display: 'inline-block',
        height: '100%',
        padding: '9px 12px',
        width: 'auto'
      })
      baseInputStyle = Object.assign({}, baseInputStyle, {
        width: '100%',
        padding: '5px 10px 0px 10px',
        position: 'relative',
        left: '-1px'
      })
    }

    const inputGroupStyle = Object.assign({}, baseInputGroupStyle, this.props.inputGroupStyle)
    const spanStyle = Object.assign({}, baseSpanStyle, this.props.spanStyle)
    const inputStyle = Object.assign({}, baseInputStyle, this.props.inputStyle)
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
