import React from 'react'

class LoadableOverlay extends React.Component {
  propTypes: {
    loaded: React.PropTypes.bool.isRequired
  }

  defaultProps: {
    loaded: false
  }

  render () {
    let element = !this.props.loaded ? (
      <div className='overlay'>
        <i className='fa fa-refresh fa-spin' />
      </div>
        ) : <div style={{ display: 'none' }} />
    return element
  }
}

export default LoadableOverlay
