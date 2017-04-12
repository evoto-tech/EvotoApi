import React from 'react'
import PropTypes from 'prop-types'

class Box extends React.Component {
  render () {
    return (
      <div className={`box box-${this.props.type} ${this.props.solid ? 'box-solid' : ''}`}>
        <div className='box-header with-border'>
          {!this.props.title ? '' : (
            <h3 className='box-title' style={{ width: '100%' }}>{this.props.title}</h3>
          )}
          {!this.props.subtitle ? '' : (
            <small style={{ marginLeft: '0.5em' }} className='box-subtitle'>{this.props.subtitle}</small>
          )}
        </div>
        {!this.props.children ? '' : (
          <div className='box-body'>
            {this.props.children}
          </div>
        )}
        {!this.props.overlay ? '' : this.props.overlay}
        {!this.props.footer ? '' : (
          <div className='box-footer'>
            {this.props.footer}
          </div>
        )}
      </div>
    )
  }
}

Box.propTypes = {
  type: PropTypes.string,
  title: PropTypes.node,
  subtitle: PropTypes.string,
  solid: PropTypes.bool,
  children: PropTypes.node,
  footer: PropTypes.node,
  overlay: PropTypes.node
}

export default Box
