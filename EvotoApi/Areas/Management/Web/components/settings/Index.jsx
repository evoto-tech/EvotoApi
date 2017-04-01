import React from 'react'
import Wrapper from '../parts/Wrapper.jsx'
import RegisterEnabled from './parts/RegisterEnabled.jsx'
import CustomFields from './parts/CustomFields.jsx'

class SettingsIndex extends React.Component {
  componentDidMount () {
    $('input').iCheck({
      checkboxClass: 'icheckbox_flat-green',
      radioClass: 'icheckbox_flat-green'
    })
  }

  render () {
    return (
      <Wrapper
        title='Settings'
        description='Settings for the evoto client and end users.'
      >
        <RegisterEnabled />
        <CustomFields />
      </Wrapper>
    )
  }
}

export default SettingsIndex
