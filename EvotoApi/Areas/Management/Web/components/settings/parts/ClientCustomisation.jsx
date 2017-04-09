import React from 'react'
import Box from '../../parts/Box.jsx'
import LabelledOverlay from '../../parts/LabelledOverlay.jsx'

class RegisterEnabled extends React.Component {
  render () {
    const overlay = (
      <LabelledOverlay
        label='Work in progress'
      />
    )
    return (
      <Box
        type='success'
        title='Client Customisation'
        overlay={overlay}
       />
    )
  }
}

export default RegisterEnabled
