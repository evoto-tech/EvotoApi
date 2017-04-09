import React from 'react'

class LabelledOverlay extends React.Component {
  render () {
    const style = {
      position: 'absolute',
      top: '50%',
      left: '50%',
      transform: 'translate(-50%, -50%)',
      fontWeight: 'bold'
    }
    return (
      <div className='overlay'>
        <span style={style}>{this.props.label}</span>
      </div>
    )
  }
}

LabelledOverlay.propTypes = {
  label: React.PropTypes.string
}

export default LabelledOverlay
