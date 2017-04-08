import React from 'react'
import Box from '../../parts/Box.jsx'
import NamedInput from '../../parts/form-components/NamedInput.jsx'

class RegisterEnabled extends React.Component {
  componentDidMount () {
    $(this.enableRegistrationCheckbox).iCheck({
      checkboxClass: 'icheckbox_flat-green',
      radioClass: 'icheckbox_flat-green'
    })
  }

  render () {
    return (
      <Box
        type='success'
        title='Enable User Registration in Client'
      >
        <form>
          <NamedInput
            name='Enable user registration through the evoto client'
            type='checkbox'
            className='icheckbox_flat-green'
            spanStyle={{ width: 'auto' }}
            inputRef={(input) => { this.enableRegistrationCheckbox = input }}
          />
        </form>
      </Box>
    )
  }
}

export default RegisterEnabled
