import React from 'react'
import PropTypes from 'prop-types'

class LoadableOverlay extends React.Component {
  render () {
    let element = !this.props.loaded ? (
      <div className='overlay'>
        <i className='fa fa-refresh fa-spin' />
      </div>
        ) : <div style={{ display: 'none' }} />
    return element
  }
}

LoadableOverlay.defaultProps = {
  loaded: false
}

LoadableOverlay.propTypes = {
  loaded: PropTypes.bool.isRequired
}

export default LoadableOverlay
