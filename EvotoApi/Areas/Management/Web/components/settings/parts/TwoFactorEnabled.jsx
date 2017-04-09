import React from 'react'
import Box from '../../parts/Box.jsx'
import NamedInput from '../../parts/form-components/NamedInput.jsx'
import LabelledOverlay from '../../parts/LabelledOverlay.jsx'

class TwoFactorEnabled extends React.Component {
  componentDidMount () {
    $(this.enable2FA).iCheck({
      checkboxClass: 'icheckbox_flat-green',
      radioClass: 'icheckbox_flat-green'
    })
  }

  render () {
    const overlay = (
      <LabelledOverlay
        label='Work in progress'
      />
    )
    return (
      <Box
        type='success'
        title='Enable Two Factor Authentication'
        overlay={overlay}
      >
        <form>
          <NamedInput
            name='Enable Two Factor Authentication for the client'
            type='checkbox'
            className='icheckbox_flat-green'
            spanStyle={{ width: 'auto' }}
            inputRef={(input) => { this.enable2FA = input }}
          />
        </form>
      </Box>
    )
  }
}

export default TwoFactorEnabled
