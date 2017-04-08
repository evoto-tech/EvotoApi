import React from 'react'
import Box from '../../parts/Box.jsx'
import NamedInput from '../../parts/form-components/NamedInput.jsx'

class TwoFactorEnabled extends React.Component {
  componentDidMount () {
    $(this.enable2FA).iCheck({
      checkboxClass: 'icheckbox_flat-green',
      radioClass: 'icheckbox_flat-green'
    })
  }

  render () {
    return (
      <Box
        type='success'
        title='Enable Two Factor Authentication'
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
