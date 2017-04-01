import React from 'react'
import Box from '../../parts/Box.jsx'

class RegisterEnabled extends React.Component {
  render () {
    return (
      <Box
        type='success'
        title='Enable User Registration in Client'
      >
        <form>
          <label>
            <input type='checkbox' name='iCheck' className='icheckbox_flat-green' />
            <span style={{ position: 'relative', left: '5px', top: '0.14em' }}>Enable user registration through the evoto client</span>
          </label>
        </form>
      </Box>
    )
  }
}

export default RegisterEnabled
