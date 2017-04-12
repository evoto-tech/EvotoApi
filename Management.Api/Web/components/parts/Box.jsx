import React from 'react'

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
  type: React.PropTypes.string,
  title: React.PropTypes.node,
  subtitle: React.PropTypes.string,
  solid: React.PropTypes.bool,
  children: React.PropTypes.node,
  footer: React.PropTypes.node,
  overlay: React.PropTypes.node
}

export default Box
